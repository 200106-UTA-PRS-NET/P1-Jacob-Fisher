using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P1.Models;

namespace P1.Controllers
{
    [Route("Order/Builder/[action]")]
    [Authorize(Roles = "User")]
    public class OrderBuilderController : Controller
    {
        private readonly ILogger<OrderBuilderController> _logger;
        private readonly IPizzaRepository _context;
        private UserManager<IdentityUser> _mgr;

        public OrderBuilderController(ILogger<OrderBuilderController> logger, IPizzaRepository context, UserManager<IdentityUser> mgr)
        {
            _logger = logger;
            _context = context;
            _mgr = mgr;
        }

        // GET: OrderBuilder
        public ActionResult Index()
        {
            var uid = _mgr.GetUserId(this.User);
            var login = _context.GetLogins(uid);
            Order order = _context.CurrentOrder((User)login);
            return View(new OrderBuilderView(order));
        }

        // GET: OrderBuilder/Details/5
        public ActionResult Details(int id)
        {
            var uid = _mgr.GetUserId(this.User);
            var login = _context.GetLogins(uid);
            IPizza p = _context.GetCurrentPizza(login, id);
            var pizza = new PizzaView(p);
            return View(pizza);
        }

        // GET: OrderBuilder/Create
        public ActionResult Create()
        {
            var uid = _mgr.GetUserId(this.User);
            var login = _context.GetLogins(uid);
            var order = _context.CurrentOrderLazy(login);
            Pizza pizza = new Pizza(order)
            {
                Crust = new Crust() { Id = 1 },
                Size = new Size() { Id = 1}
            };
            pizza.AddTopping(new Topping() { Id = 1 });
            pizza.AddTopping(new Topping() { Id = 2 });
            pizza.AddTopping(new Topping() { Id = 1 });
            _context.AddPizza(login, pizza);
            return Redirect(nameof(Index));
            //return View();
        }

        // POST: OrderBuilder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderBuilder/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderBuilder/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderBuilder/Delete/5
        public ActionResult Delete(int id)
        {
            var uid = _mgr.GetUserId(this.User);
            var login = _context.GetLogins(uid);
            IPizza p = _context.GetCurrentPizza(login, id);
            var pizza = new PizzaView(p);
            return View(pizza);
        }

        // POST: OrderBuilder/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var uid = _mgr.GetUserId(this.User);
                var login = _context.GetLogins(uid);
                _context.RemovePizzaFromOrder(login, id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(IFormCollection collection)
        {
            try
            {
                var uid = _mgr.GetUserId(this.User);
                var login = _context.GetLogins(uid);
                _context.ConfirmOrder((User)login);
                return Redirect("/Home/Index");
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}