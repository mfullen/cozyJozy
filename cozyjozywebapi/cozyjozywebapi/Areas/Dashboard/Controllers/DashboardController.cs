using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cozyjozywebapi.Areas.Dashboard.Controllers
{
    
    public class DashboardController : Controller
    {
        // GET: Dashboard/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}