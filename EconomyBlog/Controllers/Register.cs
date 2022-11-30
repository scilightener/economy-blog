using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;

namespace EconomyBlog.Controllers;

[HttpController("register")]
public class RegisterController
{
    [HttpGET]
    public ActionResult RegisterUser(string login, string password)
    {
        throw new NotImplementedException();
    }
}