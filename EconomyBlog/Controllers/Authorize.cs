using System.Net;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.ORM;

namespace EconomyBlog.Controllers;

[HttpController("auth")]
public class AuthorizeController : Controller
{
    [HttpPOST]
    public static ActionResult LoginUser(string login, string password)
    {
        throw new NotImplementedException();
        var result = new ActionResult();
        var dao = new UserDao();
        dao.Insert(login, password);
        result.StatusCode = HttpStatusCode.Redirect;
        result.RedirectUrl = @"http://localhost:7700/feed";
        return result;
    }

    [HttpGET]
    public static ActionResult GetLoginPage(string path) => ProcessStatic("auth", path);
}