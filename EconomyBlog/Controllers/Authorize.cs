using EconomyBlog.Services;

namespace EconomyBlog.Controllers;

[HttpController("auth")]
public class AuthorizeController : Controller
{
    [HttpPOST]
    public static ActionResult LoginUser(Guid sessionId, string login, string password, string rememberMe = "off")
    {
        var dao = new UserDao();
        User? user;
        try
        {
            user = dao.GetByLoginPassword(login, Hashing.Hash(password));
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }

        if (user is null)
            return new UnauthorizedResult(UserNotFound);


        var result = new ActionResult
        {
            StatusCode = HttpStatusCode.Redirect,
            RedirectUrl = @"/feed/",
            Cookies = new CookieCollection
            {
                new Cookie("SessionId",
                        SessionManager.CreateSession(user.Id, login, DateTime.Now, rememberMe == "on").ToString(), "/")
                    { Expires = rememberMe == "on" ? DateTime.Now.AddDays(150) : DateTime.Now.AddHours(1) }
            }
        };
        return result;
    }

    [HttpGET]
    public static ActionResult GetLoginPage(Guid sessionId, string path) => ProcessStatic("auth", path);
}