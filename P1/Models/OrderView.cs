using Domain;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace P1.Models
{
    public class OrderView
    {
        readonly IOrder _order;
        public long id { get => _order.Id; }
        [Display(Name = "Store")]
        public string Store { get => _order.Store.Name; }
        [Display(Name = "User")]
        public string User { get => _order.User.Username; }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Price { get => _order.Price; }
        [Display(Name = "Number of Pizzas")]
        public int PizzaCount { get => _order.Pizzas.Count(); }
        [Display(Name = "Order Time")]
        public DateTime Ordertime { get => _order.Ordertime; }
        public OrderView(IOrder order)
        {
            _order = order;
        }
    }
}
