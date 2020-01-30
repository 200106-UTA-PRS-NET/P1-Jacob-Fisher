using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Prebuilt
    {
        public Prebuilt()
        {
            Prebuilt1 = new HashSet<Prebuilt1>();
            PrebuiltToppings = new HashSet<PrebuiltToppings>();
        }

        public short Id { get; set; }
        public short CrustId { get; set; }
        public string Name { get; set; }

        public virtual Crust Crust { get; set; }
        public virtual ICollection<Prebuilt1> Prebuilt1 { get; set; }
        public virtual ICollection<PrebuiltToppings> PrebuiltToppings { get; set; }
    }
}
