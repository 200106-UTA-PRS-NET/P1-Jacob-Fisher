using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Incomplete
    {
        public Incomplete()
        {
            IncompletePizza = new HashSet<IncompletePizza>();
        }

        public int Userid { get; set; }
        public int Storeid { get; set; }

        public virtual Store Store { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<IncompletePizza> IncompletePizza { get; set; }
    }
}
