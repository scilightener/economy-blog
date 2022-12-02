using System.Net;
using EconomyBlog.ActionResults;
using EconomyBlog.Structures;

namespace EconomyBlog.Controllers;

public abstract class Controller
{
    internal static ActionResult ProcessStatic(string controllerName, string path)
    {
        var filePath = $"./Views/{controllerName}/{path}";
        byte[] buffer;
        if (Directory.Exists(filePath) && File.Exists(filePath + "/index.html"))
            buffer = File.ReadAllBytes(filePath + "/index.html");
        else if (!File.Exists(filePath))
            return new ActionResult {StatusCode = HttpStatusCode.NotFound};
        else buffer = File.ReadAllBytes(filePath);
        return new ActionResult
        {
            StatusCode = HttpStatusCode.OK,
            ContentType = ContentTypeProvider.GetContentType(path),
            Buffer = buffer
        };
    }
}