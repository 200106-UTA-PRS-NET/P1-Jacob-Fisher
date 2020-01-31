using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace P1.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            string Error = TempData["ErrorString"] as string;
            string ReturnString = TempData["ReturnString"] as string;
            ViewBag.ErrorString = Error;
            ViewBag.ReturnString = ReturnString;
            return View();
        }
    }
}