using System.Data.SqlClient;
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
    public static ActionResult RegisterUser(Guid sessionId, string login, string password, string rememberMe = "off")
    {
        var dao = new UserDao();
        int id;
        try
        {
            id = dao.Insert(login, Hashing.Hash(password));
        }
        catch (SqlException ex)
        {
            return ex.Class switch
            {
                14 => new ErrorResult($"Login '{login}' is already taken. Consider another variant and try again."),
                16 => new ErrorResult($"Too long login. Only logins with maximum length 20 characters allowed."),
                _ => new ErrorResult(DbError)
            };
        }

        return new ActionResult
        {
            StatusCode = HttpStatusCode.Redirect,
            RedirectUrl = @"/home/edit/",
            Cookies = new CookieCollection
            {
                new Cookie("SessionId",
                        SessionManager.CreateSession(id, login, DateTime.Now, rememberMe == "on").ToString(), "/")
                    { Expires = rememberMe == "on" ? DateTime.Now.AddDays(150) : DateTime.Now.AddHours(1) }
            }
        };
    }

    [HttpGET]
    public static ActionResult GetRegisterPage(Guid sessionId, string path) => ProcessStatic("register", path);
}