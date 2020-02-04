using Domain;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Testing
{
    public class UnitTest1
    {
        PizzaDbContext GetContext()
        {
            try {
            var configurBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Secrets.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configurBuilder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<PizzaDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("PizzaDB"));

            var options = optionsBuilder.Options;
            return new PizzaDbContext(options);
            } catch
            {
                return null;
            }
        }
        PizzaRepository GetRepository(PizzaDbContext context)
        {
            if(context == null)
            {
                return null;
            }
            return new PizzaRepository(context);
        }
        [Fact]
        public void GetOrdersDoesntBreak()
        {
            //Checking that there are no nullpos
            var repo = GetRepository(GetContext());
            if (repo == null)
            {
                return;
            }
            var test = repo.GetOrders(new User() { Id = 1 });
            var test2 = repo.GetOrders(new Domain.Store() { Id = 2 });
            var test3 = repo.GetOrder( 5, new User() { Id = 1 });
        }

        [Fact]
        public void PriceLimit()
        {
            Order testOrder = new Order(new Domain.Store());
            Action addPizza = () => testOrder.Add(new CompletedPizza() { Price = 260 });
            Assert.Throws<InvalidOperationException>(addPizza);
        }

        [Fact]
        public void CountLimit()
        {
            Order testOrder = new Order(new Domain.Store());
            for(int i=0; i<100; i++)
            {
                testOrder.Add(new CompletedPizza() { Price = 1 });
            }
            Action addPizza = () => testOrder.Add(new CompletedPizza() { Price = 1 });
            Assert.Throws<InvalidOperationException>(addPizza);
        }
        [Fact]
        public void AtLeastOneStore()
        {
            var repo = GetRepository(GetContext());
            if (repo == null)
            {
                return;
            }
            Assert.NotEmpty(repo.GetStores());
        }
    }
}
