using System.Net;
using System.Text;
using System.Text.Json;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
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
            if (!string.IsNullOrEmpty(firstName)) dao.Update("first_name", firstName, userId);
            if (!string.IsNullOrEmpty(lastName)) dao.Update("last_name", firstName, userId);
            dao.Update("age", age.ToString(), userId);
            if (!string.IsNullOrEmpty(education)) dao.Update("education", firstName, userId);
            if (!string.IsNullOrEmpty(job)) dao.Update("job", firstName, userId);
            dao.Update("risk_index", riskFactor.ToString(), userId);
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
        if (!int.TryParse(path.Split('/')[^1], out var id))
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
    public static ActionResult GetHomePage(Guid sessionId, string path) => ProcessStatic("home", path);
}