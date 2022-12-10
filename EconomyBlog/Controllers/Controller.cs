using System.Text;
using EconomyBlog.Structures;

namespace EconomyBlog.Controllers;

public abstract class Controller
{
    internal static ActionResult ProcessStatic(string controllerName, string path, object? model = null)
    {
        var filePath = $"./Views/{controllerName}/{path}";
        var buffer = Array.Empty<byte>();
        if (Directory.Exists(filePath) && File.Exists(filePath + "/index.html"))
            filePath += "/index.html";
        else if (!File.Exists(filePath))
            return new ErrorResult(FileOrDirectoryNotFound);
        else
            buffer = File.ReadAllBytes(filePath);
        return new ActionResult
        {
            StatusCode = HttpStatusCode.OK,
            ContentType = ContentTypeProvider.GetContentType(path),
            Buffer = filePath.EndsWith(".html") ? Encoding.UTF8.GetBytes(ActionResult.GetHtml(filePath, model)) : buffer
        };
    }
}