using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace P1.Models
{
    public class OrderDetailsView
    {
        readonly IOrder _order;
        readonly IEnumerable<PizzaView> _pizzas;
        [Display(Name = "Store")]
        public string Store { get => _order.Store.Name; }
        [Display(Name = "User")]
        public string User { get => _order.User.Username; }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Price { get => _order.Price; }
        [Display(Name = "Order Time")]
        public DateTime Ordertime { get => _order.Ordertime; }
        public OrderDetailsView(IOrder order)
        {
            _order = order;
            _pizzas = (order!=null)?from p in order.Pizzas select new PizzaView(p):null;
        }
        public IEnumerable<PizzaView> Pizzas { get => _pizzas; }
    }
}
