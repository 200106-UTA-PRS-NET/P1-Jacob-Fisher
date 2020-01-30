using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Interfaces
{
    public interface IOrder
    {
        public Store Store { get; }
        public User User { get; }
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal Price { get; }
        IEnumerable<IPizza> Pizzas { get; }
        public DateTime Ordertime { get; }
    }
}
