using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public partial class Logins
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Aspnetuserguid { get; set; }

        public virtual AspNetUsers Aspnetusergu { get; set; }
        public virtual Store Store { get; set; }
        public virtual Users Users { get; set; }
    }
}
