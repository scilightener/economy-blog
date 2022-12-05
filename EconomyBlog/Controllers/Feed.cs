using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;

namespace EconomyBlog.Controllers;

[HttpController("feed")]
public class FeedController : Controller
{
    [HttpGET]
    public static ActionResult GetFeedPage(Guid sessionId, string path) => ProcessStatic("feed", path);
}