using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.Structures;

namespace EconomyBlog.Controllers;

[HttpController("feed")]
public class FeedController : Controller
{
    [HttpGET]
    public static ActionResult GetFeedPage(string path) => ProcessStatic("feed", path);
}