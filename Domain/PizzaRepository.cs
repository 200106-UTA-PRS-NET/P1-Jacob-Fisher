﻿using Domain.Interfaces;
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
        private PizzaDbContext context;
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

        public Logins GetLogins(string login)
        {
            return PizzaMapper.Map(
                context.Logins.Where(o => o.Aspnetuserguid == context.AspNetUsers.Where(o => o.Email==login).Select(o => o.Id).FirstOrDefault()).Include(o => o.Store).Include(o => o.Users).FirstOrDefault()
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
    }
}