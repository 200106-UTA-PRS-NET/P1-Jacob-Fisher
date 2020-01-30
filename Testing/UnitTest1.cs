using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;

namespace Testing
{
    public class UnitTest1
    {
        PizzaDbContext GetContext()
        {
            var configurBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Secrets.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configurBuilder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<PizzaDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("PizzaDB"));

            var options = optionsBuilder.Options;
            return new PizzaDbContext(options);
        }
        PizzaRepository GetRepository(PizzaDbContext context)
        {
            return new PizzaRepository(context);
        }
        [Fact]
        public void Test1()
        {
            //Checking that there are no nullpos
            var repo = GetRepository(GetContext());
            var test = repo.GetOrders(new User() { Id = 1 });
            var test2 = repo.GetOrders(new Domain.Store() { Id = 2 });
            var test3 = repo.GetOrder( 5, new User() { Id = 1 });
        }
    }
}
