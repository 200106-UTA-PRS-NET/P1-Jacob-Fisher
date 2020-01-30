using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class PizzaToppings
    {
        public long OrderId { get; set; }
        public int PizzaNum { get; set; }
        public short ToppingId { get; set; }
        public byte Amount { get; set; }

        public virtual Pizza Pizza { get; set; }
        public virtual Topping Topping { get; set; }
    }
}
