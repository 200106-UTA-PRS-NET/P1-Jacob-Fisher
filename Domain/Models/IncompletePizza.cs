using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class IncompletePizza
    {
        public IncompletePizza()
        {
            IncompleteToppings = new HashSet<IncompleteToppings>();
        }

        public int Id { get; set; }
        public int PizzaId { get; set; }
        public short CrustId { get; set; }
        public short Size { get; set; }

        public virtual Crust Crust { get; set; }
        public virtual Incomplete IdNavigation { get; set; }
        public virtual Size SizeNavigation { get; set; }
        public virtual ICollection<IncompleteToppings> IncompleteToppings { get; set; }
    }
}
