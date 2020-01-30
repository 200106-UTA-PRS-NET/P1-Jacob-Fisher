using Domain.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Store: Logins, IPricer
    {
        static decimal CalculatePrice(short crustId, short sizeId, int toppingCount)
        {
            decimal price = 4.00M;
            decimal toppingPrice = 1.00M;
            switch (crustId)
            {
                case 1:
                case 2:
                    {
                        switch (sizeId)
                        {
                            case 1:
                                {
                                    price = 2.00M;
                                    toppingPrice = 0.50M;
                                    break;
                                }
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                                {
                                    price += 2.00M * (sizeId - 2);
                                    break;
                                }
                        }
                        break;
                    }
                case 3:
                case 4:
                    {
                        switch (sizeId)
                        {
                            case 1:
                                {
                                    price = 5.00M;
                                    toppingPrice = 0.50M;
                                    break;
                                }
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                                {
                                    price = 6.00M + 2.50M * (sizeId - 2);
                                    break;
                                }
                        }
                        break;
                    }
            }
            price += toppingCount*toppingPrice;
            return price;
        }
        public string Name { get; set; }
        public virtual Location Location { get; set; }

        public override bool IsStore => true;

        public decimal Price(IPizza pizza)
        {
            return CalculatePrice(pizza.Crust.Id, pizza.Size.Id, pizza.Toppings.Count());
        }
        internal decimal PriceIf(IPizza pizza, int toppingCount)
        {
            return CalculatePrice(pizza.Crust.Id, pizza.Size.Id, toppingCount);
        }
        internal decimal PriceIf(IPizza pizza, Crust crust)
        {
            return CalculatePrice(crust.Id, pizza.Size.Id, pizza.Toppings.Count());
        }
        internal decimal PriceIf(IPizza pizza, Size size)
        {
            return CalculatePrice(pizza.Crust.Id, size.Id, pizza.Toppings.Count());
        }
    }
}
