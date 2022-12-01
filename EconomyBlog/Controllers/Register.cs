using System.Net;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.ORM;
using EconomyBlog.Structures;

namespace EconomyBlog.Controllers;

[HttpController("register")]
public class RegisterController : Controller
{
    [HttpPOST]
    public static ActionResult RegisterUser(string login, string password)
    {
        var result = new ActionResult();
        var dao = new UserDao();
        dao.Insert(login, password);
        result.StatusCode = HttpStatusCode.Redirect;
        result.RedirectUrl = @"http://localhost:7700/home/edit";
        return result;
    }

    [HttpGET]
    public static ActionResult GetRegisterPage(string path) => ProcessStatic("register", path);
}