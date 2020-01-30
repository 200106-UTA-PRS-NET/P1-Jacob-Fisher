using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class IncompleteToppings
    {
        public int Id { get; set; }
        public int Pizzaid { get; set; }
        public short Toppingid { get; set; }
        public byte Amount { get; set; }

        public virtual IncompletePizza IncompletePizza { get; set; }
        public virtual Topping Topping { get; set; }
    }
}
