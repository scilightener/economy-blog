using System.Net;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.ORM;
using EconomyBlog.Structures;

namespace EconomyBlog.Controllers;

[HttpController("auth")]
public static class AuthorizeController
{
    [HttpPOST]
    public static ActionResult LoginUser(string login, string password)
    {
        throw new NotImplementedException();
        var result = new ActionResult();
        var dao = new UserDao();
        dao.Insert(login, password);
        result.StatusCode = HttpStatusCode.Redirect;
        result.RedirectUrl = @"http://localhost:7700/feed";
        return result;
    }

    [HttpGET]
    public static ActionResult GetLoginPage(string path) => ProcessStatic(path);

    private static ActionResult ProcessStatic(string path) =>
        new()
        {
            StatusCode = HttpStatusCode.OK,
            ContentType = ContentTypeProvider.GetContentType(path),
            Buffer = File.ReadAllBytes($"./Views/auth/{path}" + (path.Contains('.') ? "" : "/index.html"))
        };
}