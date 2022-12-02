using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;

namespace EconomyBlog.Controllers;

[HttpController("home")]
public class HomeController : Controller
{
    [HttpGET]
    public static ActionResult GetHomePage(string path) => ProcessStatic("home", path);
}