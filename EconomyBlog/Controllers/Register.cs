using System.Net;
using System.Web;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.ORM;
using EconomyBlog.ServerLogic.SessionLogic;
using static EconomyBlog.Messages;
using Cookie = System.Net.Cookie;

namespace EconomyBlog.Controllers;

[HttpController("register")]
public class RegisterController : Controller
{
    [HttpPOST]
    public static ActionResult RegisterUser(Guid sessionId, string login, string password/*, bool rememberMe = false*/)
    {
        var dao = new UserDao();
        int id;
        try
        {
            // check if login is already taken
            id = dao.Insert(login, HttpUtility.UrlDecode(password));
        }
        catch
        {
            return new ErrorResult(ServerFault);
        }
        return new ActionResult
        {
            StatusCode = HttpStatusCode.Redirect,
            RedirectUrl = @"/home/edit/",
            Cookies = new CookieCollection
            {
                new Cookie("SessionId", SessionManager.CreateSession(id, login, DateTime.Now).ToString(), "/")
                    // { Expires = rememberMe ? DateTime.Now.AddMonths(3) : DateTime.Now.AddHours(1)}
                    { Expires = DateTime.Now.AddHours(1) }
            }
        };
    }

    [HttpGET]
    public static ActionResult GetRegisterPage(Guid sessionId, string path) => ProcessStatic("register", path);
}