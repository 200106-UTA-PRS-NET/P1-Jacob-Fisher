using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    class CompletedOrder : IOrder
    {
        public decimal Price { get; set;}

        public IEnumerable<IPizza> Pizzas { get; set; }

        public Store Store { get; set; }

        public User User { get; set; }
        public DateTime Ordertime { get; set; }
    }
}
