using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Users
    {
        public Users()
        {
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }

        public virtual Logins IdNavigation { get; set; }
        public virtual Incomplete Incomplete { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
