using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace P1.Models
{
    public class NumericPizzaView
    {
        IPizza _pizza;
        public int SizeId { get => _pizza.Size?.Id??0; }
        public int CrustId { get => _pizza.Crust?.Id??0; }
        public int Topping1 { get => _pizza.Toppings.ElementAtOrDefault(0)?.Id ?? 0; }
        public int Topping2 { get => _pizza.Toppings.ElementAtOrDefault(1)?.Id ?? 0; }
        public int Topping3 { get => _pizza.Toppings.ElementAtOrDefault(2)?.Id ?? 0; }
        public int Topping4 { get => _pizza.Toppings.ElementAtOrDefault(3)?.Id ?? 0; }
        public int Topping5 { get => _pizza.Toppings.ElementAtOrDefault(4)?.Id ?? 0; }
        public NumericPizzaView(IPizza pizza)
        {
            _pizza = pizza;
        }
    }
}
