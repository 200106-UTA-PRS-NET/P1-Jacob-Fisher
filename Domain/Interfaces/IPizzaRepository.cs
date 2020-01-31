using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IPizzaRepository
    {
        IEnumerable<IOrder> GetOrders(Logins login);
        IOrder GetOrder(long id, Logins login);
        Order CurrentOrder(User user);
        void ConfirmOrder(User user);
        IEnumerable<Store> GetStores();
        Store GetStore(int id);
        Logins GetLogins(string guid);
        void NewOrder(int userId, int storeId);
        Order CurrentOrderLazy(Logins login);
        IPizza GetCurrentPizza(Logins login, int id);
        void AddPizza(Logins login, IPizza pizza);
        void RemovePizzaFromOrder(Logins login, int pizzaId);
    }
}
