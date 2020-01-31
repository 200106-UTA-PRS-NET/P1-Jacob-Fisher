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
            IDictionary<short, string> preBuilts = _context.GetPrebuiltNames(order.Store);
            preBuilts.Add(0, "Custom");
            ViewBag.PreBuilts = preBuilts;
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
        public ActionResult Create(short id)
        {
            IDictionary<short, string> sizes = _context.GetSizeNames();
            IDictionary<short, string> crusts = _context.GetCrustNames();
            IDictionary<short, string> toppings = _context.GetToppingNames();
            toppings.Add(0, "None");
            ViewBag.Sizes = sizes;
            ViewBag.Crusts = crusts;
            ViewBag.Toppings = toppings;

            var prebuilt = _context.GetPrebuilt(id);
            prebuilt.Size = new Size() { Id = 3 };
            var viewValue = new NumericPizzaView(prebuilt);
            return View(viewValue);
        }

        // POST: OrderBuilder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(short SizeId, short CrustId, short T1, short T2, short T3, short T4, short T5, IFormCollection collection)
        {
            try
            {
                var uid = _mgr.GetUserId(this.User);
                var login = _context.GetLogins(uid);
                var order = _context.CurrentOrderLazy(login);
                var newPizza = new Pizza(order)
                {
                    Size = new Size() { Id = SizeId },
                    Crust = new Crust() { Id = CrustId },
                };
                if(T1 != 0) newPizza.AddTopping(new Topping() { Id = T1 });
                if (T2 != 0) newPizza.AddTopping(new Topping() { Id = T2 });
                if (T3 != 0) newPizza.AddTopping(new Topping() { Id = T3 });
                if (T4 != 0) newPizza.AddTopping(new Topping() { Id = T4 });
                if (T5 != 0) newPizza.AddTopping(new Topping() { Id = T5 });
                _context.AddPizza(login, newPizza);
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
            var uid = _mgr.GetUserId(this.User);
            var login = _context.GetLogins(uid);
            IDictionary<short, string> sizes = _context.GetSizeNames();
            IDictionary<short, string> crusts = _context.GetCrustNames();
            IDictionary<short, string> toppings = _context.GetToppingNames();
            toppings.Add(0, "None");
            ViewBag.Sizes = sizes;
            ViewBag.Crusts = crusts;
            ViewBag.Toppings = toppings;
            ViewBag.PizzaId = id;

            var current = _context.GetCurrentPizza(login,id);
            var viewValue = new NumericPizzaView(current);
            return View(viewValue);
        }

        // POST: OrderBuilder/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, short SizeId, short CrustId, short T1, short T2, short T3, short T4, short T5, IFormCollection collection)
        {
            try
            {
                var uid = _mgr.GetUserId(this.User);
                var login = _context.GetLogins(uid);
                var order = _context.CurrentOrderLazy(login);
                var newPizza = new Pizza(order)
                {
                    Size = new Size() { Id = SizeId },
                    Crust = new Crust() { Id = CrustId },
                };
                if (T1 != 0) newPizza.AddTopping(new Topping() { Id = T1 });
                if (T2 != 0) newPizza.AddTopping(new Topping() { Id = T2 });
                if (T3 != 0) newPizza.AddTopping(new Topping() { Id = T3 });
                if (T4 != 0) newPizza.AddTopping(new Topping() { Id = T4 });
                if (T5 != 0) newPizza.AddTopping(new Topping() { Id = T5 });

                _context.UpdatePizza(login, id, newPizza);

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