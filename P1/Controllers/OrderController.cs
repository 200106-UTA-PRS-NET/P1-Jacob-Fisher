using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P1.Models;

namespace P1.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IPizzaRepository _context;
        private UserManager<IdentityUser> _mgr;
        public OrderController(ILogger<OrderController> logger, IPizzaRepository context, UserManager<IdentityUser> mgr)
        {
            _logger = logger;
            _context = context;
            _mgr = mgr;
        }
        // GET: Order
        [Authorize(Roles = "User,Store")]
        public ActionResult Index()
        {
            var id = _mgr.GetUserId(this.User);
            var login = _context.GetLogins(id);
            var orders = from order in _context.GetOrders(login) select new OrderView(order);
            return View(orders);
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            var uid = _mgr.GetUserId(this.User);
            var login = _context.GetLogins(uid);
            var order = _context.GetOrder(id, login);
            return View(new OrderDetailsView(order));
        }

        // GET: Order/New/5
        [Authorize(Roles = "User")]
        public ActionResult New(int id)
        {
            var uid = _mgr.GetUserId(this.User);
            var login = _context.GetLogins(uid);
            _context.NewOrder(login.Id, id);
            return Redirect("/Order/Builder/Index");
        }
    }
}