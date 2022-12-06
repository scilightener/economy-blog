using System.Net;
using System.Web;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.Models;
using EconomyBlog.ORM;
using EconomyBlog.ServerLogic.SessionLogic;
using static EconomyBlog.Messages;

namespace EconomyBlog.Controllers;

[HttpController("auth")]
public class AuthorizeController : Controller
{
    [HttpPOST]
    public static ActionResult LoginUser(Guid sessionId, string login, string password)
    {
        var dao = new UserDao();
        User? user;
        try
        {
            user = dao.GetByLoginPassword(HttpUtility.UrlDecode(login), HttpUtility.UrlDecode(password));
        }
        catch
        {
            return new ErrorResult(ServerFault);
        }

        if (user is null)
            return new UnauthorizedResult("There's no such a user. Please register.");

        
        var result = new ActionResult
        {
            StatusCode = HttpStatusCode.Redirect,
            RedirectUrl = @"/feed/",
            Cookies = new CookieCollection
            {
                new Cookie("SessionId", SessionManager.CreateSession(user.Id, login, DateTime.Now).ToString(), "/")
                    // { Expires = rememberMe ? DateTime.Now.AddMonths(3) : DateTime.Now.AddHours(1)}
                    { Expires = DateTime.Now.AddHours(1) }
            }
        };
        return result;
    }

    [HttpGET]
    public static ActionResult GetLoginPage(Guid sessionId, string path) => ProcessStatic("auth", path);
}