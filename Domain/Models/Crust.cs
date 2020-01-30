using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Crust
    {
        public Crust()
        {
            CrustInventory = new HashSet<CrustInventory>();
            IncompletePizza = new HashSet<IncompletePizza>();
            Pizza = new HashSet<Pizza>();
            Prebuilt = new HashSet<Prebuilt>();
        }

        public short Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<CrustInventory> CrustInventory { get; set; }
        public virtual ICollection<IncompletePizza> IncompletePizza { get; set; }
        public virtual ICollection<Pizza> Pizza { get; set; }
        public virtual ICollection<Prebuilt> Prebuilt { get; set; }
    }
}
