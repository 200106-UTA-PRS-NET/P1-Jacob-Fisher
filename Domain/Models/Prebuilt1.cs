using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Prebuilt1
    {
        public int StoreId { get; set; }
        public short PrebuiltId { get; set; }

        public virtual Prebuilt Prebuilt { get; set; }
        public virtual Store Store { get; set; }
    }
}
