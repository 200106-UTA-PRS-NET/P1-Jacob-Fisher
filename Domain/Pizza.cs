using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Domain
{
    public class Pizza : IPizza
    {
        private readonly IOrder order;
        private readonly ICollection<Topping> toppings;
        public int Id { get; set; }
        public Crust Crust { get; set; }
        public Size Size { get; set; }
        public decimal Price => order.Store.Price(this);
        public decimal PriceIf(int toppingCount)
        {
            return order.Store.PriceIf(this, toppingCount);
        }
        public decimal PriceIf(Crust crust)
        {
            return order.Store.PriceIf(this, crust);
        }
        public decimal PriceIf(Size size)
        {
            return order.Store.PriceIf(this, size);
        }
        public IEnumerable<Topping> Toppings => toppings;
        public void AddTopping(Topping topping)
        {
            if (topping == null)
            {
                throw new ArgumentNullException("topping");
            }
            if (toppings.Count < 5)
            {
                toppings.Add(topping);
            } else
            {
                throw new InvalidOperationException("Each pizza may only have 5 toppings.");
            }
        }
        public void RemoveTopping(Topping topping)
        {
            var topp = (from top in toppings where top.Id == topping.Id select top).FirstOrDefault();
            toppings.Remove(topp);
        }
        public Pizza(IOrder order)
        {
            this.order = order;
            Crust = new Crust()
            {
                Id = 1
            };
            Size = new Size()
            {
                Id = 1
            };
            toppings = new List<Topping>();
        }
        public Pizza(IOrder order, IPizza pizza)
        {
            this.order = order;
            Crust = pizza.Crust;
            Size = pizza.Size;
            toppings = new List<Topping>(pizza.Toppings);
        }
        public override string ToString()
        {
            return $"{Price:C}: {Size.Name} {Crust.Name} pizza with {string.Join(", ", from topping in Toppings select topping.Name)}";
        }
    }
}