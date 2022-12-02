using System.Net;
using System.Web;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.ORM;
using EconomyBlog.ServerLogic.SessionLogic;
using Cookie = System.Net.Cookie;

namespace EconomyBlog.Controllers;

[HttpController("register")]
public class RegisterController : Controller
{
    [HttpPOST]
    public static ActionResult RegisterUser(string login, string password/*, bool rememberMe = false*/)
    {
        var dao = new UserDao();
        var id = -1;
        try
        {
            // check if login is already taken
            id = dao.Insert(login, HttpUtility.UrlDecode(password));
        }
        catch
        {
            return new ActionResult();
        }
        var result = new ActionResult
        {
            StatusCode = HttpStatusCode.Redirect,
            RedirectUrl = @"/home/edit/",
            Cookies = new CookieCollection
            {
                new Cookie("SessionId", SessionManager.CreateSession(id, login, DateTime.Now).ToString(), "/")
            }
        };
        return result;
    }

    [HttpGET]
    public static ActionResult GetRegisterPage(string path) => ProcessStatic("register", path);
}