using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Domain
{
    public class ContextDBBacked : IContext
    {
        static readonly DbContextOptions<PizzaDbContext> options;
        static ContextDBBacked()
        {
            var configurBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Secrets.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configurBuilder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<PizzaDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("PizzaDB"));

            options = optionsBuilder.Options;
        }
        // TODO: replace all references to DB with this
        private PizzaDbContext ConnectDB()
        {
            return new PizzaDbContext(options);
        }
        private ContextDBBacked()
        {
            db = ConnectDB();
        }
        public static ContextDBBacked Context { get => Nested.instance; }
        Store location = null;
        public Store Location { get { if (location != null) { return location; } else if (IsStore) { return ((Store)LoggedIn); } else { return null; } } }

        public Logins LoggedIn => loggedIn;
        bool IsUser => loggedIn!=null&&loggedIn.IsUser;
        bool IsStore => loggedIn != null && loggedIn.IsStore;

        public bool InOrder => currentOrder != null;
        public bool InPizza => currentPizza != null;

        private PizzaDbContext db;
        Order currentOrder;
        Pizza currentPizza;
        Logins loggedIn;
        Topping GetTopping(string topping)
        {
            return PizzaMapper.Map(db.Topping.First(t => t.Name == topping));
        }

        //public void Login(string name, string password)
        //{
        //    if (LoggedIn != null)
        //    {
        //        throw new InvalidOperationException($"You are already logged in as {LoggedIn.Username}");
        //    }
        //    var temp = (from login in db.Logins
        //     where login.Username.Equals( name )&& login.Password.Equals(password)
        //     select login).Include(l => l.Users).Include(l => l.Store).FirstOrDefault();
        //    loggedIn = PizzaMapper.Map(temp);
        //    if(LoggedIn == null)
        //    {
        //        throw new PizzaBoxException("Username and password do not match.");
        //    }
        //}

        public IEnumerable<string> GetToppings()
        {
            var toppings = from topping in db.Topping
                           select PizzaMapper.Map(topping).Name;
            if (toppings == null)
            {
                throw new PizzaBoxException("Couldn't retrieve topping list.");
            }
            return toppings;
        }

        public IEnumerable<Topping> GetToppings(IEnumerable<string> toppings)
        {
            return from topping in db.Topping
                   where toppings.Contains(topping.Name.ToLower())
                   select PizzaMapper.Map(topping);
        }

        public IEnumerable<string> GetStores()
        {
            return from store in db.Store
                   select PizzaMapper.Map(store).Name;
        }
        Store GetStore(string name)
        {
            return (from store in db.Store
                   where store.Name.ToLower()==name.ToLower()
                   select PizzaMapper.Map(store)).FirstOrDefault();
        }
        public void SetStore(string store)
        {
            if (IsStore)
            {
                throw new InvalidOperationException("You cannot change stores while logged in as a store.");
            }
            if (currentOrder != null)
            {
                throw new InvalidOperationException("You cannot change stores while in the middle of an order.");
            }
            location = GetStore(store);
        }
        public IEnumerable<string> GetPresets()
        {
            if (Location == null)
            {
                throw new InvalidOperationException("Cannot get preset list before setting the store.");
            }
            return from preset in db.Prebuilt
                   join prebuiltId in from preset in db.Prebuilt1
                                       where preset.StoreId == Location.Id
                                       select preset.PrebuiltId
                                       on preset.Id equals prebuiltId
                   select preset.Name.Trim();
        }

        //public void Register(string name, string password)
        //{
        //    if (loggedIn != null) { throw new InvalidOperationException("Cannot register if already logged in."); }
        //    Models.Logins logins = new Models.Logins() { Username = name, Password = password };
        //    Models.Users users = new Users() { IdNavigation = logins };
        //        try {
        //            db.Logins.Add(logins);
        //            db.Users.Add(users);
        //            db.SaveChanges();
        //        }
        //        catch (DbUpdateException)
        //        {
        //            db.Dispose();
        //            db = ConnectDB();
        //            throw new PizzaBoxException("A user with that username already exists.");
        //        }
        //}

        public void Logout()
        {
            if(loggedIn == null)
            {
                throw new InvalidOperationException("You can't logout; you aren't logged in.");
            }
            CancelOrder();
            loggedIn = null;
            location = null;
        }

        public void NewOrder()
        {
            if (!IsUser)
            {
                throw new InvalidOperationException("Cannot create an order if you aren't logged in as a user.");
            }
            var lastOrder = PizzaMapper.Map(db.Orders.Where(o => o.Userid == LoggedIn.Id).Select(o => o).OrderByDescending(o => o.Ordertime).FirstOrDefault());
            if (lastOrder != null) {
                double timeDif = (DateTime.Now - lastOrder.Ordertime).TotalHours;
                if (timeDif < 2)
                {
                    throw new InvalidOperationException("You cannot order more frequently than once/2hrs.");
                }
                else if (timeDif < 24 && Location.Id != lastOrder.Store.Id) {
                    throw new InvalidOperationException("You cannot order from different stores in a 24hr window.");
                }
            }
            currentOrder = new Order(Location);
        }

        public void NewOrder(IEnumerable<IPizza> pizzas)
        {
            NewOrder();
            foreach(var pizza in pizzas)
            {
                AddPizza(pizza);
            }
        }
        void AddPizza(IPizza pizza)
        {
            currentOrder.Add(pizza);
        }
        public IOrder PreviewOrder()
        {
            return currentOrder;
        }
        public void PlaceOrder()
        {
            PlaceOrder(false);
        }
        public void PlaceOrder(bool ignoreInventory)
        {
            if (!IsUser) { throw new InvalidOperationException("You need to login as a user to place an order.");}
            if (currentOrder.Pizzas.Count() < 1) { throw new InvalidOperationException("You need to have at least one pizza in your order."); }
            currentOrder.User = (User)loggedIn;
            IDictionary<int, int> idToCount;
            IDictionary<string, int> nameToCount;
            (idToCount, nameToCount) = currentOrder.GetToppingCounts();
            
            foreach(var topping in from topping in db.Topping
                                   where nameToCount.Keys.Contains(topping.Name.Trim())
                                   select topping)
            {
                try
                {
                    idToCount[topping.Id] += nameToCount[topping.Name.Trim()];
                } catch
                {
                    idToCount[topping.Id] = nameToCount[topping.Name.Trim()];
                }
                nameToCount.Remove(topping.Name);
            }
            if (nameToCount.Count > 0)
            {
                throw new PizzaBoxException("Bad topping name in topping list");
            }
            if (!ignoreInventory)
            {
                var inventory = from topping in db.ToppingInventory
                                where (topping.StoreId == Location.Id) && idToCount.Keys.Contains(topping.ToppingId)
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
                    db.ToppingInventory.Add(new ToppingInventory() { StoreId = Location.Id, ToppingId = 0, Amount = -1 });
                }
            }
            db.Orders.Add(PizzaMapper.Map(currentOrder));

            try
            {
                db.SaveChanges();
                currentOrder = null;
            } catch (DbUpdateException)
            {
                db.Dispose();
                db = ConnectDB();
                throw new PizzaBoxException("Was unable to place order. This is likely due to the store not having the toppings required to complete your order.");
            }
        }

        public IEnumerable<IOrder> OrderHistory()
        {
            if (LoggedIn == null)
            {
                throw new InvalidOperationException("Must be logged in to retrieve order history.");
            }
            if (IsStore) {
                return db.Orders.Where(o => o.Storeid == LoggedIn.Id).Include(o => o.User).Include("User.IdNavigation").Include(o => o.Pizza).Include("Pizza.Crust").Include("Pizza.PizzaToppings").Include("Pizza.PizzaToppings.Topping").Include("Pizza.SizeNavigation").Select(o => PizzaMapper.Map(o));
            }
            else if (IsUser)
            {
                return db.Orders.Where(o => o.Userid == LoggedIn.Id).Include(o => o.Store).Include("Store.IdNavigation").Include(o => o.Pizza).Include("Pizza.Crust").Include("Pizza.PizzaToppings").Include("Pizza.PizzaToppings.Topping").Include("Pizza.SizeNavigation").Select(o => PizzaMapper.Map(o));
            }
            throw new PizzaBoxException("Couldn't retrieve order history.");
        }
        Crust GetCrust(string crust)
        {
            return PizzaMapper.Map(db.Crust.First(t => t.Name.ToLower() == crust.ToLower()));
        }
        Size GetSize(string size)
        {
            return PizzaMapper.Map(db.Size.First(t => t.Name.ToLower() == size.ToLower()));
        }
        IPizza GetPreset(string name)
        {
            return PizzaMapper.Map(db.Prebuilt.Include(p => p.PrebuiltToppings).Include("PrebuiltToppings.Topping").Include(p=>p.Crust).First(t => t.Name == name));
        }
        public void NewPizza()
        {
            currentPizza = new Pizza(currentOrder)
            {
                Crust = GetCrust("Regular"),
                Size = GetSize("Medium")
            };
            AddTopping("sauce");
            AddTopping("cheese");
        }

        public void AddPizzaToOrder()
        {
            AddPizzaToOrder(1);
        }

        public void AddTopping(Topping topping)
        {
            currentPizza.AddTopping(topping);
        }
        public void AddTopping(string topping)
        {
            AddTopping(GetTopping(topping));
        }
        public IEnumerable<User> GetUsers()
        {
            if (!IsStore) { throw new InvalidOperationException("You may only perform this operation if you are logged in as a store."); }
            var users = (from user in db.Users
                   join orderUserId in (from order in db.Orders
                                        where order.Storeid == LoggedIn.Id
                                        select order.Userid).Distinct()
                                      on user.Id equals orderUserId
                   select user).Include(u => u.IdNavigation);
            return from user in users select PizzaMapper.Map(user);
        }

        public void SetCrust(string crust)
        {
            currentPizza.Crust = GetCrust(crust);
        }

        public void SetSize(string size)
        {
            currentPizza.Size = GetSize(size);
        }

        public void PresetPizza(string preset)
        {
            IPizza pizza = GetPreset(preset);
            currentPizza = new Pizza(currentOrder)
            {
                Crust = pizza.Crust,
                Size = GetSize("Medium"),
            };
            foreach(var topping in pizza.Toppings)
            {
                currentPizza.AddTopping(topping);
            }
        }

        public void CancelOrder()
        {
            currentOrder = null;
            CancelPizza();
        }

        public IEnumerable<string> GetCrusts()
        {
            return from crust in db.Crust
                   select crust.Name.Trim();
        }

        public IEnumerable<string> GetSizes()
        {
            return from size in db.Size
                   select size.Name.Trim();
        }

        public IPizza PreviewPizza()
        {
            return currentPizza;
        }

        public void CancelPizza()
        {
            currentPizza = null;
        }

        public void RemoveTopping(string topping)
        {
            currentPizza.RemoveTopping(GetTopping(topping));
        }

        public void AddPizzaToOrder(uint count)
        {
            if (!InPizza)
            {
                throw new InvalidOperationException("Cannot add pizza to order. Not building pizza.");
            }
            if (count == 0)
            {
                throw new ArgumentOutOfRangeException("Cannot add 0 pizzas to the order.");
            }
            for(int i=1; i<count; i++){
                currentOrder.Add(new Pizza(currentOrder, currentPizza));
            }
            currentOrder.Add(currentPizza);
            currentPizza = null;
        }

        public IEnumerable<Sales> GetSalesMonths()
        {
            return GetSalesMonths(12);
        }
                           
        public IEnumerable<Sales> GetSalesDays()
        {
            return GetSalesDays(30);
        }
                           
        public IEnumerable<Sales> GetSalesMonths(int numMonths)
        {
            if (LoggedIn == null||!IsStore)
            {
                throw new InvalidOperationException("Must be logged in as a store to retrieve sales.");
            }
            return (from order in db.Orders
                    where ((DateTime.Now.Year - order.Ordertime.Year) * 12 + DateTime.Now.Month - order.Ordertime.Month)<numMonths
                    select new { order, count = order.Pizza.Count() }).AsEnumerable().GroupBy(o => new DateTime(o.order.Ordertime.Year, o.order.Ordertime.Month, 1)).OrderByDescending(m => m.Key).Select(
                                   months => new Sales()
                                   {
                                       Time = months.Key,
                                       Value = months.Sum(m => m.order.Price),
                                       NumPizzas = months.Sum(m => m.count)
                                   });

        }
        public IEnumerable<Sales> GetSalesDays(int numDays)
        {
            if (LoggedIn == null || !IsStore)
            {
                throw new InvalidOperationException("Must be logged in as a store to retrieve sales.");
            }
            return (from order in db.Orders
                    where ((DateTime.Now.Year - order.Ordertime.Year) * 365 + DateTime.Now.DayOfYear-order.Ordertime.DayOfYear) < numDays
                    select new { order, count = order.Pizza.Count() }).AsEnumerable().GroupBy(o => new DateTime(o.order.Ordertime.Year, o.order.Ordertime.Month, o.order.Ordertime.Day)).OrderByDescending(m => m.Key).Select(
                                   days => new Sales()
                                   {
                                       Time = days.Key,
                                       Value = days.Sum(m => m.order.Price),
                                       NumPizzas = days.Sum(m => m.count)
                                   });
        }

        public int GetInventory(string toppingName)
        {
            if (!IsStore)
            {
                throw new InvalidOperationException("Need to be logged in as a store to get inventory.");
            }
            var top = GetTopping(toppingName);
            var val =(from topping in db.ToppingInventory
                   where topping.StoreId == loggedIn.Id && topping.ToppingId == top.Id
                   select topping.Amount).First();
            if (val != null)
            {
                return (int)val;
            } else
            {
                return 0;
            }
        }

        private class Nested
        {
            // Nested class for Singleton Pattern
            static Nested() { }
            internal static ContextDBBacked instance = new ContextDBBacked();
        }
    }
}