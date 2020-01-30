using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Store
    {
        public Store()
        {
            CrustInventory = new HashSet<CrustInventory>();
            Incomplete = new HashSet<Incomplete>();
            Orders = new HashSet<Orders>();
            Prebuilt1 = new HashSet<Prebuilt1>();
            ToppingInventory = new HashSet<ToppingInventory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Location { get; set; }

        public virtual Logins IdNavigation { get; set; }
        public virtual Location LocationNavigation { get; set; }
        public virtual ICollection<CrustInventory> CrustInventory { get; set; }
        public virtual ICollection<Incomplete> Incomplete { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<Prebuilt1> Prebuilt1 { get; set; }
        public virtual ICollection<ToppingInventory> ToppingInventory { get; set; }
    }
}
