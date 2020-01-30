using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    abstract public class Logins
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public virtual bool IsUser => false;
        public virtual bool IsStore => false;
    }
}
