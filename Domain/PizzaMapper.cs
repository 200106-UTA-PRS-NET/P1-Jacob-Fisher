using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    static class PizzaMapper
    {
        internal static Domain.Topping Map(Domain.Models.Topping topping)
        {
            if (topping == null)
            {
                return null;
            }
            return new Domain.Topping()
            {
                Name = topping.Name.Trim(),
                Id = topping.Id
            };
        }
        internal static Domain.Models.Topping Map(Domain.Topping topping)
        {
            if (topping == null)
            {
                return null;
            }
            return new Domain.Models.Topping()
            {
                Name = topping.Name,
                Id = topping.Id
            };
        }
        internal static Domain.Store Map(Domain.Models.Store store)
        {
            if (store == null)
            {
                return null;
            }
            return new Domain.Store()
            {
                Id = store.Id,
                Name = store.Name.Trim(),
                Location = Map(store.LocationNavigation),
                Username = store.IdNavigation?.Username.Trim()
            };
        }
        internal static Domain.Logins Map(Domain.Models.Logins logins)
        {
            if (logins == null)
            {
                return null;
            }
            if (logins.Store != null)
            {
                return Map(logins.Store);
            }
            if (logins.Users != null)
            {
                return Map(logins.Users);
            }
            throw new ArgumentException("Logins is neither a store nor a user");
        }
        internal static Domain.Location Map(Domain.Models.Location location)
        {
            if (location == null)
            {
                return null;
            }
            return new Domain.Location()
            {
                Id = location.Id,
                Name = location.Name.Trim()
            };
        }
        internal static Domain.Models.Store Map(Domain.Store store)
        {
            if (store == null)
            {
                return null;
            }
            return new Domain.Models.Store()
            {
                Id = store.Id,
                Name = store.Name,
                Location = store.Location.Id
            };
        }
        internal static Domain.Models.Location Map(Domain.Location location)
        {
            if (location == null)
            {
                return null;
            }
            return new Domain.Models.Location()
            {
                Id = location.Id,
                Name = location.Name
            };
        }
        internal static Domain.Models.Logins Map(Domain.Logins login)
        {
            if (login == null)
            {
                return null;
            }
            return new Domain.Models.Logins()
            {
                Id = login.Id,
                Username = login.Username
            };
        }
        internal static Domain.User Map(Domain.Models.Users login)
        {
            if (login == null)
            {
                return null;
            }
            return new Domain.User()
            {
                Id = login.Id,
                Username = login.IdNavigation.Username.Trim()
            };
        }
        internal static IOrder Map(Domain.Models.Orders orders)
        {
            if (orders == null)
            {
                return null;
            }
            return new CompletedOrder()
            {
                Id = orders.Id,
                Store = Map(orders.Store),
                User = Map(orders.User),
                Price = orders.Price,
                Ordertime = orders.Ordertime,
                Pizzas = from pizza in orders.Pizza
                         select PizzaMapper.Map(pizza)
            };
        }
        internal static Order Map(Domain.Models.Incomplete incomplete)
        {
            if(incomplete == null)
            {
                return null;
            } else
            {
                var ret = new Order(Map(incomplete.Store));
                foreach (var p in incomplete.IncompletePizza)
                {
                    ret.Add(Map(p, ret));
                }
                return ret;
            }
        }
        internal static IPizza Map(Domain.Models.IncompletePizza incomplete, Order o)
        {
            if (incomplete == null)
            {
                return null;
            }
            else
            {
                var p = new Pizza(o)
                {
                    Id = incomplete.PizzaId,
                    Crust = Map(incomplete.Crust),
                    Size = Map(incomplete.SizeNavigation),
                };
                foreach(var t in incomplete.IncompleteToppings)
                {
                    for (int i = 0; i < t.Amount; i++) {
                        p.AddTopping(Map(t.Topping));
                    }
                }
                return p;
            }
        }

        internal static IPizza Map(Domain.Models.Pizza pizza)
        {
            if (pizza == null)
            {
                return null;
            }
            List < Topping >  toppings = new List<Topping>();
            foreach(var t in pizza.PizzaToppings)
            {
                Topping topp = Map(t.Topping);
                for(int i=0; i<t.Amount; i++)
                {
                    toppings.Add(topp);
                }
            }
            return new CompletedPizza()
            {
                Crust = PizzaMapper.Map(pizza.Crust),
                Price = pizza.Price,
                Size = PizzaMapper.Map(pizza.SizeNavigation),
                Toppings = toppings
            };
        }

        internal static Size Map(Models.Size sizeNavigation)
        {
            if (sizeNavigation == null)
            {
                return null;
            }
            return new Size()
            {
                Id = sizeNavigation.Id,
                Name = sizeNavigation.Name
            };
        }

        internal static Crust Map(Models.Crust crust)
        {
            if (crust == null)
            {
                return null;
            }
            return new Crust()
            {
                Id = crust.Id,
                Name = crust.Name.Trim()
            };
        }
        internal static Domain.Models.Orders Map(IOrder order)
        {
            if (order == null)
            {
                return null;
            }
            var o = new Domain.Models.Orders()
            {
                Price = order.Price,
                Storeid = order.Store.Id,
                Userid = order.User.Id,
            };
            int i = 0;
            foreach(var pizza in order.Pizzas)
            {
                var mappedPizza = Map(pizza);
                mappedPizza.PizzaNum = ++i;
                o.Pizza.Add(mappedPizza);
            }
            return o;
        }

        internal static Models.Pizza Map(IPizza pizza)
        {
            if (pizza == null)
            {
                return null;
            }
            var p = new Models.Pizza()
            {
                CrustId = pizza.Crust.Id,
                Price = pizza.Price,
                Size = pizza.Size.Id,

            };
            foreach(var topping in (from topping in pizza.Toppings
                                   select topping).Distinct())
            {
                p.PizzaToppings.Add(new PizzaToppings()
                {
                    Amount = (byte)(from t in pizza.Toppings
                                    where t.Id == topping.Id
                                    select t).Count(),
                    ToppingId = topping.Id,
                });
            }
            return p;
        }

        internal static Users Map(User user)
        {
            if (user == null)
            {
                return null;
            }
            return new Users()
            {
                Id = user.Id,
            };
        }

        internal static IPizza Map(Prebuilt prebuilt)
        {
            if (prebuilt == null)
            {
                return null;
            }
            return new CompletedPizza()
            {
                Crust = PizzaMapper.Map(prebuilt.Crust),
                Toppings = new List<Topping>((from topping in prebuilt.PrebuiltToppings
                                              select Map(topping.Topping)))
            };
        }
    }
}
