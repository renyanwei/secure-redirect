using SecureRedirect.Core;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace SecureRedirect.Web.Attribute
{
    public class SecureRequestAttribute : AuthorizeAttribute
    {
        public string PrivateKey { get; set; }
        public SecureRequestAttribute(string privatekey)
        {
            PrivateKey = privatekey;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpRequestBase Request = filterContext.RequestContext.HttpContext.Request;
            NameValueCollection parameters = new NameValueCollection()
              {
                Request.Form,
                Request.QueryString
              };

            string sign = parameters["sign"];
            parameters.Remove("sign");
            List<string> paramlist = new List<string>();
            foreach (var item in parameters.AllKeys.OrderBy(k => k))
            {
                paramlist.Add(item + "=" + HttpUtility.UrlDecode(parameters[item]));
            }
            string presignstr = string.Join("&", paramlist);
            string digest = RSAHelper.DecryptString(sign, ConfigurationManager.AppSettings[PrivateKey]);

            if (Sha1.Compute(presignstr) != digest)
            {
                ContentResult result = new ContentResult();
                result.Content = "Sign Error.";
                filterContext.Result = result;
            }

        }
    }
}
