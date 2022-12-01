using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;

namespace EconomyBlog.Controllers;

[HttpController("global")]
public static class GlobalController
{
    [HttpGET]
    public static ActionResult GetGlobalCss(string path) => !path.EndsWith("global.css")
        ? new ActionResult()
        : new ActionResult { ContentType = "text/css", Buffer = File.ReadAllBytes($"./Views/global/{path}") };
}