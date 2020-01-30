using System;
using System.Collections.Generic;
using System.Text;
using Domain.Interfaces;

namespace Domain
{
    public class Order : IOrder
    {
        const uint maxCount = 100;
        const decimal maxPrice = 250.00M;
        readonly ICollection<IPizza> pizzas;
        decimal price;
        Store store;
        public decimal Price { get => price; }
        public IEnumerable<IPizza> Pizzas { get => pizzas; }

        public Store Store { get => store; set { store = value; } }

        public User User { get; set; }
        public DateTime Ordertime { get => DateTime.Now; }

        public void Add(IPizza pizza)
        {
            decimal price = pizza.Price;
            if (price + this.price > maxPrice)
            {
                throw new InvalidOperationException("Price too high");
            }
            else if (pizzas.Count >= maxCount)
            {
                throw new InvalidOperationException("Too many pizzas");
            }
            else
            {
                this.price += price;
                pizzas.Add(pizza);
            }
        }
        internal Order(Store store)
        {
            this.store = store;
            pizzas = new List<IPizza>();
        }
        internal Order(Store store, ICollection<IPizza> pizzas):this(store)
        {
            foreach(var pizza in pizzas)
            {
                this.Add(pizza);
            }
        }
        internal (IDictionary<int, int>, IDictionary<string, int>) GetToppingCounts()
        {
            Dictionary<int, int> idToCount = new Dictionary<int, int>();
            Dictionary<string, int> nameToCount = new Dictionary<string, int>();
            foreach (var pizza in this.Pizzas)
            {
                foreach (var topping in pizza.Toppings)
                {
                    if (topping.Id > 0)
                    {
                        try
                        {
                            idToCount[topping.Id]++;
                        }
                        catch
                        {
                            idToCount[topping.Id] = 1;
                        }
                    }
                    else
                    {
                        try
                        {
                            nameToCount[topping.Name]++;
                        }
                        catch
                        {
                            nameToCount[topping.Name] = 1;
                        }
                    }
                }
            }
            return (idToCount, nameToCount);
        }
    }
}
