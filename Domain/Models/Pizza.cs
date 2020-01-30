using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Pizza
    {
        public Pizza()
        {
            PizzaToppings = new HashSet<PizzaToppings>();
        }

        public long OrderId { get; set; }
        public int PizzaNum { get; set; }
        public short CrustId { get; set; }
        public short Size { get; set; }
        public decimal Price { get; set; }

        public virtual Crust Crust { get; set; }
        public virtual Orders Order { get; set; }
        public virtual Size SizeNavigation { get; set; }
        public virtual ICollection<PizzaToppings> PizzaToppings { get; set; }
    }
}
