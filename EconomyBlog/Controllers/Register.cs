using System.Net;
using System.Web;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.ORM;

namespace EconomyBlog.Controllers;

[HttpController("register")]
public class RegisterController : Controller
{
    [HttpPOST]
    public static ActionResult RegisterUser(string login, string password)
    {
        var dao = new UserDao();
        try
        {
            dao.Insert(login, HttpUtility.UrlDecode(password));
        }
        catch
        {
            return new ActionResult();
        }
        var result = new ActionResult
        {
            StatusCode = HttpStatusCode.Redirect,
            RedirectUrl = @"http://localhost:7700/home/edit/",
            
        };
        return result;
    }

    [HttpGET]
    public static ActionResult GetRegisterPage(string path) => ProcessStatic("register", path);
}