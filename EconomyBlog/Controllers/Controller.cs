using System.Net;
using EconomyBlog.ActionResults;
using EconomyBlog.Structures;
using static EconomyBlog.Messages;

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
            return new ErrorResult(FileOrDirectoryNotFound);
        else buffer = File.ReadAllBytes(filePath);
        return new ActionResult
        {
            StatusCode = HttpStatusCode.OK,
            ContentType = ContentTypeProvider.GetContentType(path),
            Buffer = buffer
        };
    }
}