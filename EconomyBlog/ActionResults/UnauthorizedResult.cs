using System.Net;
using System.Text;
using static EconomyBlog.Messages;

namespace EconomyBlog.ActionResults;

public class UnauthorizedResult : ActionResult
{
    public UnauthorizedResult(string message = UnauthorizedAccess)
    {
        ContentType = "text/html";
        StatusCode = HttpStatusCode.Unauthorized;
        Buffer = Encoding.UTF8.GetBytes(GetHtml("./Views/global/error/index.sbnhtml", new{ ErrorText = message}));
    }
}