using System.Net;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.ORM;
using EconomyBlog.Structures;

namespace EconomyBlog.Controllers;

[HttpController("home")]
public class HomeController : Controller
{
    [HttpGET]
    public static ActionResult GetHomePage(string path) => ProcessStatic("home", path);
}