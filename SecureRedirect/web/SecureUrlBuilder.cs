using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.ComponentModel;
using System.Web;
using System.Collections.Specialized;
using SecureRedirect.Core;

namespace SecureRedirect.Web
{
    public class SecureUrlBuilder
    {
        public static string Create(string urlpath, object querystring, object form, string publickey)
        {
            List<string> paramlist = new List<string>();
            List<string> signparam = new List<string>();
            NameValueCollection QueryString = new NameValueCollection();
            NameValueCollection Form = new NameValueCollection();
            if (querystring != null)
            {
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(querystring))
                {
                    object obj = propertyDescriptor.GetValue(querystring);
                    paramlist.Add(propertyDescriptor.Name + "=" + HttpUtility.UrlEncode(obj.ToString()));
                    QueryString.Add(propertyDescriptor.Name, obj.ToString());
                }
            }
            if (form != null)
            {
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(form))
                {
                    object obj = propertyDescriptor.GetValue(form);
                    paramlist.Add(propertyDescriptor.Name + "=" + HttpUtility.UrlEncode(obj.ToString()));
                    Form.Add(propertyDescriptor.Name, obj.ToString());
                }

            }
            NameValueCollection parameters = new NameValueCollection()
              {
                Form,
                QueryString
              };
            foreach (var item in parameters.AllKeys.OrderBy(k => k))
            {
                signparam.Add(item + "=" + parameters[item]);
            }

            string digest = Sha1.Compute(string.Join("&", signparam));
            paramlist.Add("sign=" + RSAHelper.EncryptString(digest, publickey));
            return urlpath + (paramlist.Count > 0 ? "?" + string.Join("&", paramlist) : "");
        }
    }
}
