using System.Net;
using System.Web;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.Models;
using EconomyBlog.ORM;
using EconomyBlog.ServerLogic.SessionLogic;

namespace EconomyBlog.Controllers;

[HttpController("auth")]
public class AuthorizeController : Controller
{
    [HttpPOST]
    public static ActionResult LoginUser(string login, string password)
    {
        var dao = new UserDao();
        Console.WriteLine(dao.GetAll().Count());
        User? user;
        try
        {
            user = dao.GetAll().FirstOrDefault(acc => acc.Login == login && acc.Password == HttpUtility.UrlDecode(password));
        }
        catch
        {
            return new ActionResult();
        }

        if (user is null)
            return new ActionResult();
        
        var result = new ActionResult
        {
            StatusCode = HttpStatusCode.Redirect,
            RedirectUrl = @"/feed/",
            Cookies = new CookieCollection
            {
                new Cookie("SessionId", SessionManager.CreateSession(user.Id, login, DateTime.Now).ToString(), "/")
            }
        };
        return result;
    }

    [HttpGET]
    public static ActionResult GetLoginPage(string path) => ProcessStatic("auth", path);
}