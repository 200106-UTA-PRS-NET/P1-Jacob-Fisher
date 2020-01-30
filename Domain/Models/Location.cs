using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Location
    {
        public Location()
        {
            Store = new HashSet<Store>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Store> Store { get; set; }
    }
}
