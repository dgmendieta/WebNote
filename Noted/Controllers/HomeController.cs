using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Noted.Controllers
{
    public class HomeController : Controller
    {
        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        } //end About

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        } //end Contact
    }
}