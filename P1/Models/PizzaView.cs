using Domain;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace P1.Models
{
    public class PizzaView
    {
        IPizza _pizza;
        public PizzaView(IPizza pizza)
        {
            _pizza = pizza;
        }
        public int Id => _pizza.Id;
        public string Size { get => _pizza.Size.Name; }
        public string Crust { get => _pizza.Crust.Name; }
        public string Toppings { get => String.Join(", ", from t in _pizza.Toppings select t.Name); }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Price { get => _pizza.Price; }
    }
}
