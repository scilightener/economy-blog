using System.Net;
using System.Text;
using EconomyBlog.ActionResults;
using EconomyBlog.Structures;
using static EconomyBlog.Messages;

namespace EconomyBlog.Controllers;

public abstract class Controller
{
    internal static ActionResult ProcessStatic(string controllerName, string path, object? model = null)
    {
        var filePath = $"./Views/{controllerName}/{path}";
        var buffer = Array.Empty<byte>()
        ;
        if (Directory.Exists(filePath) && File.Exists(filePath + "/index.sbnhtml"))
            filePath += "/index.sbnhtml";
        else if (!File.Exists(filePath))
            return new ErrorResult(FileOrDirectoryNotFound);
        else
            buffer = File.ReadAllBytes(filePath);
        return new ActionResult
        {
            StatusCode = HttpStatusCode.OK,
            ContentType = ContentTypeProvider.GetContentType(path),
            Buffer = buffer.Length > 0 ? buffer : Encoding.UTF8.GetBytes(ActionResult.GetHtml(filePath, model))
        };
    }
}