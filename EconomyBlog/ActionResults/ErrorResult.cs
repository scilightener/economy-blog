using System.Net;
using System.Text;

namespace EconomyBlog.ActionResults;

public class ErrorResult : ActionResult
{
    public ErrorResult(string message)
    {
        ContentType = "text/plain";
        StatusCode = HttpStatusCode.NotFound;
        Buffer = Encoding.UTF8.GetBytes(message);
    }
}