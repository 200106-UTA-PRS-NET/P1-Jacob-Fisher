using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IContext
    {
        Store Location { get; }
        Logins LoggedIn { get; }
        bool InOrder { get; }
        bool InPizza { get; }
        IEnumerable<string> GetToppings();
        IEnumerable<Topping> GetToppings(IEnumerable<string> toppings);
        IEnumerable<string> GetStores();
        void SetStore(string name);
        IEnumerable<string> GetPresets();
        //void Login(string name, string password);
        //void Register(string name, string password);
        //void Logout();
        void NewOrder();
        void NewOrder(IEnumerable<IPizza> pizzas);
        IOrder PreviewOrder();
        IPizza PreviewPizza();
        void PlaceOrder();
        void CancelOrder();
        void CancelPizza();
        IEnumerable<IOrder> OrderHistory();
        void NewPizza();
        void PresetPizza(string preset);
        void AddPizzaToOrder();
        void AddTopping(string topping);
        void RemoveTopping(string topping);
        IEnumerable<string> GetCrusts();
        void SetCrust(string crust);
        IEnumerable<string> GetSizes();
        void SetSize(string size);
        public IEnumerable<User> GetUsers();
        void AddPizzaToOrder(uint count);
        IEnumerable<Sales> GetSalesMonths();
        IEnumerable<Sales> GetSalesDays();
        IEnumerable<Sales> GetSalesMonths(int numMonths);
        IEnumerable<Sales> GetSalesDays(int numDays);
        int GetInventory(string toppingName);
    }
}
