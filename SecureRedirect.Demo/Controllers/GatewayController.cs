using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecureRedirect.Web;

namespace SecureRedirect.Demo.Controllers
{
    public class GatewayController : Controller
    {
        //
        // GET: /Gateway/
        [SecureRedirect.Web.Attribute.SecureRequest("privatekey")]
        public ActionResult Index()
        {
            return Content("验证成功，可以试试随便改一下网址的参数");
        }

    }
}
