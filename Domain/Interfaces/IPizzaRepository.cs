using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IPizzaRepository
    {
        IEnumerable<IOrder> GetOrders(Logins login);
        Order CurrentOrder(User user);
        IEnumerable<Store> GetStores();
        Store GetStore(int id);
        Logins GetLogins(string login);
    }
}
