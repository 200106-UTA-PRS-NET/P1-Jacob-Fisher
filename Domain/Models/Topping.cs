using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Topping
    {
        public Topping()
        {
            IncompleteToppings = new HashSet<IncompleteToppings>();
            PizzaToppings = new HashSet<PizzaToppings>();
            PrebuiltToppings = new HashSet<PrebuiltToppings>();
            ToppingInventory = new HashSet<ToppingInventory>();
        }

        public short Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<IncompleteToppings> IncompleteToppings { get; set; }
        public virtual ICollection<PizzaToppings> PizzaToppings { get; set; }
        public virtual ICollection<PrebuiltToppings> PrebuiltToppings { get; set; }
        public virtual ICollection<ToppingInventory> ToppingInventory { get; set; }
    }
}
