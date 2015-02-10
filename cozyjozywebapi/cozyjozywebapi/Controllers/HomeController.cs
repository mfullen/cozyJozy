using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using cozyjozywebapi.Entity;
using cozyjozywebapi.Models;

namespace cozyjozywebapi.Controllers
{
    public class HomeController : Controller
    {
      

        public ActionResult Index()
        {
            return View();
        }
    }
}
