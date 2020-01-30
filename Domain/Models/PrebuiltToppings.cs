using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class PrebuiltToppings
    {
        public short PrebuiltId { get; set; }
        public short ToppingId { get; set; }
        public byte Amount { get; set; }

        public virtual Prebuilt Prebuilt { get; set; }
        public virtual Topping Topping { get; set; }
    }
}
