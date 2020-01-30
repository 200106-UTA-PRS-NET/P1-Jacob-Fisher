using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Size
    {
        public Size()
        {
            IncompletePizza = new HashSet<IncompletePizza>();
            Pizza = new HashSet<Pizza>();
        }

        public short Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<IncompletePizza> IncompletePizza { get; set; }
        public virtual ICollection<Pizza> Pizza { get; set; }
    }
}
