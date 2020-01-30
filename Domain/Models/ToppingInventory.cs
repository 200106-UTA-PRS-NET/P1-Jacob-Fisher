using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class ToppingInventory
    {
        public int StoreId { get; set; }
        public short ToppingId { get; set; }
        public int? Amount { get; set; }

        public virtual Store Store { get; set; }
        public virtual Topping Topping { get; set; }
    }
}
