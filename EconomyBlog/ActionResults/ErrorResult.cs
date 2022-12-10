using System.Net;
using System.Text;
using static EconomyBlog.Messages;

namespace EconomyBlog.ActionResults;

public class ErrorResult : ActionResult
{
    public ErrorResult(string message = UnknownError)
    {
        ContentType = "text/html";
        StatusCode = HttpStatusCode.NotFound;
        Buffer = Encoding.UTF8.GetBytes(GetHtml("./Views/global/error/index.html", new { ErrorText = message }));
    }
}