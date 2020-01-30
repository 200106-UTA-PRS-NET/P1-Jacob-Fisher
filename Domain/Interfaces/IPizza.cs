using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IPizza
    {
        public Crust Crust { get; set; }
        public Size Size { get; set; }
        public decimal Price { get; }
        IEnumerable<Topping> Toppings { get; }
    }
}
