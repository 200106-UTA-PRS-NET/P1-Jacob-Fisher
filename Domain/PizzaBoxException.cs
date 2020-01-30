using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class PizzaBoxException: Exception
    {
        internal PizzaBoxException() : base() { }
        internal PizzaBoxException(string error) : base(error) { }
    }
}
