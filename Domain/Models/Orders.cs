using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Orders
    {
        public Orders()
        {
            Pizza = new HashSet<Pizza>();
        }

        public long Id { get; set; }
        public int Storeid { get; set; }
        public int Userid { get; set; }
        public decimal Price { get; set; }
        public DateTime Ordertime { get; set; }

        public virtual Store Store { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<Pizza> Pizza { get; set; }
    }
}
