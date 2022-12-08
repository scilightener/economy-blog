using System.Data.SqlClient;
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
    [HttpPOST("^edit/$")]
    public static ActionResult UpdateUserInfo(Guid sessionId, string firstName, string lastName, int age, string education, string job,
        int riskFactor, string topic1 = "", string topic2 = "", string topic3 = "", string topic4 = "", string topic5 = "")
    {
        if (sessionId == Guid.Empty) return new UnauthorizedResult();
        var userId = SessionManager.GetSessionInfo(sessionId)?.UserId ?? -1;
        if (userId == -1) return new UnauthorizedResult();
        var dao = new UserDao();
        try
        {
            // no need to update login & password, so they're null
            dao.Update(userId, new User(userId, null, null, firstName, lastName, age, education, job, riskFactor));
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }
        return new ActionResult
        {
            StatusCode = HttpStatusCode.Redirect,
            RedirectUrl = "../",
        };
    }

    [HttpGET("^edit/$")]
    public static ActionResult GetEditPage(Guid sessionId, string path) => GetHomePage(sessionId, path);

    [HttpGET(@"^\d+/$")]
    public static ActionResult GetUser(Guid sessionId, string path)
    {
        if (sessionId == Guid.Empty || !int.TryParse(path.Split('/', StringSplitOptions.RemoveEmptyEntries)[^1], out var id))
            return new ErrorResult(UserNotFound);
        var userDao = new UserDao();
        User? user;
        try
        {
            user = userDao.GetById(id);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }

        if (user is null) return new ErrorResult(UserNotFound);
        return new ActionResult
        {
            ContentType = "application/json",
            Buffer = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(user))
        };
    }

    [HttpGET(@"^\w+/$")]
    public static ActionResult GetUserByLogin(Guid sessionId, string path)
    {
        if (sessionId == Guid.Empty)
            return new ErrorResult(UserNotFound);
        var login = path.Split('/', StringSplitOptions.RemoveEmptyEntries)[^1];
        var userDao = new UserDao();
        User? user;
        try
        {
            user = userDao.GetByLogin(login);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }
        if (user is null) return new ErrorResult(UserNotFound);
        return new ActionResult
        {
            ContentType = "application/json",
            Buffer = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(user))
        };
    }
    
    [HttpGET]
    public static ActionResult GetHomePage(Guid sessionId, string path)
    {
        if (sessionId == Guid.Empty)
            return new UnauthorizedResult();
        User? user;
        try
        {
            user = new UserDao().GetById(SessionManager.GetSessionInfo(sessionId)?.UserId ?? -1);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }
        return ProcessStatic("home", path, user);
    }
}