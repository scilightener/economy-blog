using System.Net;
using System.Text;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.Models;
using EconomyBlog.ORM;

namespace EconomyBlog.Controllers;

[HttpController("register")]
public static class RegisterController
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
    public static ActionResult GetRegisterPage()
    {
        return new ActionResult
        {
            Buffer = File.ReadAllBytes("./Views/register/index.html")
        };
    }
}