using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class User: Logins
    {
        public override bool IsUser => true;
    }
}
