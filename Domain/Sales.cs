using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public struct Sales
    {
        public DateTime Time { get; set; }
        public decimal Value { get; set; }
        public int NumPizzas { get; set; }
    }
}
