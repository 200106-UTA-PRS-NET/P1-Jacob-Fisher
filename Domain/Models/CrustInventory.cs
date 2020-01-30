using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class CrustInventory
    {
        public int StoreId { get; set; }
        public short Crustid { get; set; }
        public int? Amount { get; set; }

        public virtual Crust Crust { get; set; }
        public virtual Store Store { get; set; }
    }
}
