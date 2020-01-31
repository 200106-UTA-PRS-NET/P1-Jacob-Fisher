using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class PizzaRepository: IPizzaRepository
    {
        private readonly PizzaDbContext context;
        public PizzaRepository()
        {

        }
        public PizzaRepository(PizzaDbContext dBContext)
        {
            context = dBContext;
        }

        public Order CurrentOrder(User user)
        {
            var current = PizzaMapper.Map(context.Incomplete.Where(o => o.Userid == user.Id)
                .Include(o => o.IncompletePizza)
                    .ThenInclude(o => o.Crust)
                .Include(o => o.IncompletePizza)
                    .ThenInclude(o => o.SizeNavigation)
                .Include(o => o.IncompletePizza)
                    .ThenInclude(o => o.IncompleteToppings)
                        .ThenInclude(o => o.Topping)
                .Include(o => o.Store)
                .FirstOrDefault());
            current.User = user;
            return current;
        }
        public IEnumerable<Store> GetStores()
        {
            return context.Store
                .Include(o => o.LocationNavigation)
                .Select(o => PizzaMapper.Map(o));
        }
        public Store GetStore(int id)
        {
            return PizzaMapper.Map(context.Store
                .Include(o => o.Name)
                .Include(o => o.LocationNavigation)
                    .ThenInclude(o => o.Name).Where(o => o.Id == id).FirstOrDefault());
        }

        public Logins GetLogins(string guid)
        {
            return PizzaMapper.Map(
                context.Logins.Where(o => o.Aspnetuserguid == guid).Include(o => o.Store).Include(o => o.Users).FirstOrDefault()
                );
        }

        public IEnumerable<IOrder> GetOrders(Logins login)
        {
            if (login.IsStore)
            {
                return context.Orders.Where(o => o.Storeid == login.Id)
                .Include(o => o.User)
                    .ThenInclude(o => o.IdNavigation)
                .Include(o => o.Pizza)
                .Select(o => PizzaMapper.Map(o));
            } else
            {
                return context.Orders.Where(o => o.Userid == login.Id)
                .Include(o => o.Store)
                    .ThenInclude(o => o.IdNavigation)
                .Include(o => o.Pizza)
                .Select(o => PizzaMapper.Map(o));
            }
        }
        public IOrder GetOrder(long id, Logins login)
        {
            return PizzaMapper.Map(context.Orders
                .Include(o => o.Store)
                    .ThenInclude(o => o.IdNavigation)
                .Include(o => o.User)
                    .ThenInclude(o => o.IdNavigation)
                .Include(o => o.Pizza)
                    .ThenInclude(o => o.PizzaToppings)
                        .ThenInclude(o => o.Topping)
                .Include(o => o.Pizza)
                    .ThenInclude(o => o.Crust)
                .Include(o => o.Pizza)
                    .ThenInclude(o => o.SizeNavigation)
                .Where(o => o.Id == id).Where(o => (o.Storeid == login.Id) || (o.Userid == login.Id)).FirstOrDefault());
        }
        public void NewOrder(int userId, int storeId)
        {
            CancelOrder(userId);
            context.Incomplete.Add(new Incomplete() {
                Userid = userId,
                Storeid = storeId
            });
            context.SaveChanges();
        }
        public void CancelOrder(int userId)
        {
            Incomplete incomplete = context.Incomplete
                .Include(o => o.IncompletePizza)
                    .ThenInclude(o=>o.IncompleteToppings)
                .Where(o => o.Userid == userId).FirstOrDefault();
            if(incomplete != null) {
                context.Remove(incomplete);
                context.SaveChanges();
            }
        }

        public Order CurrentOrderLazy(Logins login)
        {
            return PizzaMapper.Map(context.Incomplete
                .Where(o => o.Userid == login.Id).FirstOrDefault());
        }

        public IPizza GetCurrentPizza(Logins login, int id)
        {
            return PizzaMapper.Map(
                _GetCurrentPizza(login, id)
                , PizzaMapper.Map(context.Incomplete
                .Include(o => o.Store)
                .Where(o => o.Userid == login.Id).FirstOrDefault()));
        }

        private Models.IncompletePizza _GetCurrentPizza(Logins login, int id)
        {
            return context.IncompletePizza
                .Include(o => o.Crust)
                .Include(o => o.SizeNavigation)
                .Include(o => o.IncompleteToppings)
                    .ThenInclude(o => o.Topping)
                .Where(o => o.Id == login.Id && o.PizzaId == id)
                .FirstOrDefault();
        }

        public void AddPizza(Logins login, IPizza pizza)
        {
            context.IncompletePizza.Add(PizzaMapper.Map(pizza, login));
            context.SaveChanges();
        }

        public void RemovePizzaFromOrder(Logins login, int pizzaId)
        {
            var pizza = _GetCurrentPizza(login, pizzaId);
            context.Remove(pizza);
            context.SaveChanges();
        }

        public void ConfirmOrder(User user)
        {
            var currentOrder = CurrentOrder(user);
            IDictionary<int, int> idToCount;
            IDictionary<string, int> nameToCount;
            (idToCount, nameToCount) = currentOrder.GetToppingCounts();

            foreach (var topping in from topping in context.Topping
                                    where nameToCount.Keys.Contains(topping.Name.Trim())
                                    select topping)
            {
                try
                {
                    idToCount[topping.Id] += nameToCount[topping.Name.Trim()];
                }
                catch
                {
                    idToCount[topping.Id] = nameToCount[topping.Name.Trim()];
                }
                nameToCount.Remove(topping.Name);
            }
            if (nameToCount.Count > 0)
            {
                throw new PizzaBoxException("Bad topping name in topping list");
            }
            var inventory = from topping in context.ToppingInventory
                            where (topping.StoreId == currentOrder.Store.Id) && idToCount.Keys.Contains(topping.ToppingId)
                            select topping;
            try
            {
                foreach (var id in idToCount.Keys)
                {
                    inventory.First(inv => inv.ToppingId == id).Amount -= idToCount[id];
                }
            }
            catch
            {
                context.ToppingInventory.Add(new ToppingInventory() { StoreId = currentOrder.Store.Id, ToppingId = 0, Amount = -1 });
            }
            context.Orders.Add(PizzaMapper.Map(currentOrder));
            try
            {
                context.SaveChanges();
                CancelOrder(user.Id);
            }
            catch (DbUpdateException)
            {
                throw new PizzaBoxException("Was unable to place order. This is likely due to the store not having the toppings required to complete your order.");
            }
        }
    }
}
