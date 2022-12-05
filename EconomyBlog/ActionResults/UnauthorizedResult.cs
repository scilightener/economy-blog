using System.Net;
using System.Text;

namespace EconomyBlog.ActionResults;

public class UnauthorizedResult : ActionResult
{
    public UnauthorizedResult(string message)
    {
        ContentType = "text/plain";
        StatusCode = HttpStatusCode.Unauthorized;
        Buffer = Encoding.UTF8.GetBytes(message);
    }
}