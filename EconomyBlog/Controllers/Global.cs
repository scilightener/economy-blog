using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using static EconomyBlog.Messages;

namespace EconomyBlog.Controllers;

[HttpController("global")]
public class GlobalController : Controller
{
    [HttpGET]
    public static ActionResult GetGlobalCss(Guid sessionId, string path) => path.EndsWith("global.css")
        ? ProcessStatic("global", path)
        : new ErrorResult(FileOrDirectoryNotFound);
}