# secure-redirect
WEB系统之间安全加密Redirect跳转，加密采用RSA非对称加密QueryString和Form数据，主要功能包括生成加密摘要和验证加密摘要。
#项目说明
本项目比较简单，主要作用是两个系统之间传输敏感数据时不被第三方**篡改**，类似于常见的商城系统和第三方支付平台之间发送请求，请求中含有支付金额、商户标识等敏感信息，一旦被篡改的后果将会是非常严重，因此需要对消息进行加密传输，因此就有了本项目。
#项目原理
此类的需求一般会在请求的URL地址上携带一个Sign参数。就是把所有请求数据连接成一个字符串再进行然后用Sha1或者Md5算法生成消息摘要再对消息摘要进行**公钥**加密就生成了Sign。接收方在接收到数据时再根据同样的算法和步骤对参数生成消息摘要，然后再对请求方的Sign进行**私钥**解密，最后拿生成的消息摘要和请求方已解密的消息摘要进行对比，相同则表示消息在传输过程中没有遭到篡改。

> 本项目目的和作用是保证消息在WEB传输中的完整性，但是由于WEB的开放性和透明性，不能保证消息不被截获，那样已经超出了本项目所能及的范围。

#使用方法
本项目为实现以上的目的会对请求方和接收方都进行改造，以便对消息进行加密和解密。
##请求方改造
原有代码：
```C#
public ActionResult SRedirect()
{
    return Redirect("/Gateway/Index?Param1=Alice&Param2=Bob");
}
```
改造后的代码：
```C#
public ActionResult SRedirect()
{
    return Redirect(SecureUrlBuilder.Create("/Gateway/Index", new { Param1 = "Alice", Param2 = "Bob" }, null, ConfigurationManager.AppSettings["publickey"]));
}
```
改造主要是对原来要重定向的地址进行包装，里面用到了`SecureUrlBuilder.Create`方法，参数如下：
|参数|类型|说明|
|---|---|---|
|urlpath|String|URL路径（不包括参数部分）
|querystring|Object|URL参数
|form|Object|Form参数
|publickey|String|对参数加密的`公钥`
##接收方改造
原有代码：
```C#
public class GatewayController : Controller
{
    public ActionResult Index()
    {
        return Content("Hello world");
    }
}
```
改造后的代码
```C#
public class GatewayController : Controller
{
    [SecureRedirect.Web.Attribute.SecureRequest("privatekey")]
    public ActionResult Index()
    {
        return Content("验证成功，可以试试随便改一下网址的参数");
    }
}
```
改造的方式是在Action上增加了特性`[SecureRedirect.Web.Attribute.SecureRequest]`，构造方法参数如下：
|参数|类型|说明|
|---|---|---|
|privatekey|String|私钥在Web.config文件`AppSettings`中的Key|
##结束语
这个项目比较简单。且目前只引用与ASP.NET MVC项目。后期将会初步拓展。