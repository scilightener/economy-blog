using System.Net;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.ORM;
using EconomyBlog.Structures;

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
    public static ActionResult GetRegisterPage(string path) => ProcessStatic(path);

    private static ActionResult ProcessStatic(string path) =>
        new()
        {
            StatusCode = HttpStatusCode.OK,
            ContentType = ContentTypeProvider.GetContentType(path),
            Buffer = File.ReadAllBytes($"./Views/register/{path}" + (path.Contains('.') ? "" : "/index.html"))
        };
}