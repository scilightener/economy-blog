using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;

namespace EconomyBlog.Controllers;

[HttpController("global")]
public class GlobalController : Controller
{
    [HttpGET]
    public static ActionResult GetGlobalCss(string path) => path.EndsWith("global.css")
        ? ProcessStatic("global", path)
        : new ActionResult();
}