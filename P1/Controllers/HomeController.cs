using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P1.Models;

namespace P1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPizzaRepository _context;
        private UserManager<IdentityUser> _mgr;


        public HomeController(ILogger<HomeController> logger, IPizzaRepository context, UserManager<IdentityUser> mgr)
        {
            _logger = logger;
            _context = context;
            _mgr = mgr;
        }

        public IActionResult Index()
        {
            var stores = _context.GetStores().ToList();
            return View(stores);
        }

        /*[Authorize(Roles ="User,Store")]
        public IActionResult Orders()
        {
            var id = _mgr.GetUserId(this.User);
            var login = _context.GetLogins(id);
            var orders = from order in _context.GetOrders(login) select new OrderView(order);
            return View(orders);
        }*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
