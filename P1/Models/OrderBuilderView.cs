using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace P1.Models
{
    public class OrderBuilderView
    {
        readonly Order _order;
        readonly IEnumerable<PizzaView> _pizzas;
        public OrderBuilderView(Order order)
        {
            _order = order;
            _pizzas = (order != null) ? from p in order.Pizzas select new PizzaView(p) : null;
        }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Price { get => _order.Price; }
        public IEnumerable<PizzaView> Pizzas { get => _pizzas; }
    }
}
