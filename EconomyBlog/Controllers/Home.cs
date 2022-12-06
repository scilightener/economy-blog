using System.Net;
using System.Text;
using System.Text.Json;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.Models;
using EconomyBlog.ORM;
using EconomyBlog.ServerLogic.SessionLogic;
using static EconomyBlog.Messages;

namespace EconomyBlog.Controllers;

[HttpController("home")]
public class HomeController : Controller
{
    [HttpPOST("/edit")]
    public static ActionResult UpdateUserInfo(Guid sessionId, string firstName, string lastName, int age, string education, string job,
        int riskFactor, string topic1 = "", string topic2 = "", string topic3 = "", string topic4 = "", string topic5 = "")
    {
        if (sessionId == Guid.Empty) return new UnauthorizedResult(UnauthorizedAccess);
        var userId = SessionManager.GetSessionInfo(sessionId)?.UserId ?? -1;
        if (userId == -1) return new UnauthorizedResult(UnauthorizedAccess);
        var dao = new UserDao();
        try
        {
            // no need to update login & password, so they're null
            dao.Update(userId, new User(userId, null, null, firstName, lastName, age, education, job, riskFactor));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(ServerFault);
        }
        return new ActionResult
        {
            StatusCode = HttpStatusCode.Redirect,
            RedirectUrl = "../",
        };
    }

    [HttpGET(@"\d")]
    public static ActionResult GetUser(Guid sessionId, string path)
    {
        if (!int.TryParse(path.Split('/')[^2], out var id))
            return new ErrorResult(UserNotFound);
        var userDao = new UserDao();
        var user = userDao.GetById(id);
        return new ActionResult
        {
            ContentType = "application/json",
            Buffer = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(user))
        };
    }
    
    [HttpGET]
    public static ActionResult GetHomePage(Guid sessionId, string path)
    {
        
        return ProcessStatic("home", path);
    }
}