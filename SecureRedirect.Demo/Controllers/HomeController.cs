using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecureRedirect.Web;
using System.Configuration;
using System.Net;

namespace SecureRedirect.Demo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SRedirect()
        {
            return Redirect(SecureUrlBuilder.Create("/Gateway/Index", new { Param1 = "Alice", Param2 = "Bob" }, null, ConfigurationManager.AppSettings["publickey"]));
        }
    }
}
