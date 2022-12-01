using System.Net;
using EconomyBlog.ActionResults;
using EconomyBlog.Structures;

namespace EconomyBlog.Controllers;

public abstract class Controller
{
    internal static ActionResult ProcessStatic(string controllerName, string path) =>
        new()
        {
            StatusCode = HttpStatusCode.OK,
            ContentType = ContentTypeProvider.GetContentType(path),
            Buffer = File.ReadAllBytes($"./Views/{controllerName}/{path}" + (path.Contains('.') ? "" : "/index.html"))
        };
}