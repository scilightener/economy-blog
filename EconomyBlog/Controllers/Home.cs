using System.Data.SqlClient;
using System.Net;
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
        int riskFactor, int topic1 = 0, int topic2 = 0, int topic3 = 0, int topic4 = 0, int topic5 = 0)
    {
        if (sessionId == Guid.Empty) return new UnauthorizedResult();
        var userId = SessionManager.GetSessionInfo(sessionId)?.UserId ?? -1;
        if (userId == -1) return new UnauthorizedResult();
        try
        {
            // no need to update login & password, so they're null
            new UserDao().Update(userId,
                new User(userId, null, null, firstName, lastName, age, education, job, riskFactor, userId));
            new UsersFavoriteTopicsDao().Update(new UsersFavoriteTopics(userId, topic1, topic2, topic3, topic4, topic5));
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
    public static ActionResult GetEditPage(Guid sessionId, string path)
    {
        if (sessionId == Guid.Empty)
            return new UnauthorizedResult();
        IEnumerable<Topic>? topics = null;
        try
        {
            topics = new TopicsDao().GetAll().Skip(1);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            //return new ErrorResult(DbError);
        }

        return ProcessStatic("home", path, new { Topics = topics?.OrderBy(topic => topic.Name) });
    }

    [HttpGET(@"^\d+/$")]
    public static ActionResult GetUser(Guid sessionId, string path)
    {
        if (sessionId == Guid.Empty)
            return new UnauthorizedResult();
        if (!int.TryParse(path.Split('/', StringSplitOptions.RemoveEmptyEntries)[^1], out var id))
            return new ErrorResult(UserNotFound);
        User? user;
        try
        {
            user = new UserDao().GetById(id);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }

        return user is null ? new ErrorResult(UserNotFound) : ProcessStatic("home", "", user);
    }

    [HttpGET(@"^\w+/$")]
    public static ActionResult GetUserByLogin(Guid sessionId, string path)
    {
        if (sessionId == Guid.Empty)
            return new UnauthorizedResult();
        var login = path.Split('/', StringSplitOptions.RemoveEmptyEntries)[^1];
        User? user;
        try
        {
            user = new UserDao().GetByLogin(login);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }
        return user is null ? new ErrorResult(UserNotFound) : ProcessStatic("home", "", user);
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