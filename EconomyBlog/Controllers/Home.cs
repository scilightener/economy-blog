using EconomyBlog.Services;

namespace EconomyBlog.Controllers;

[HttpController("home")]
public class HomeController : Controller
{
    // TODO: change topics to strings and fix their correct work
    [HttpPOST("^edit/$")]
    public static ActionResult UpdateUserInfo(Guid sessionId, string firstName, string lastName, int age,
        string education, string job,
        int riskFactor, string topic1 = "", string topic2 = "", string topic3 = "", string topic4 = "",
        string topic5 = "")
    {
        if (sessionId == Guid.Empty) return new UnauthorizedResult();
        var userId = SessionManager.GetSessionInfo(sessionId)?.UserId ?? -1;
        if (userId == -1) return new UnauthorizedResult();
        try
        {
            // no need to update login & password, so they're null
            new UserDao().Update(userId,
                new User(userId, null, null, firstName, lastName, age, education, job, riskFactor, userId));
            var topicDao = new TopicsDao();
            new UsersFavoriteTopicsDao().Update(new UsersFavoriteTopics(userId, topicDao.GetByName(topic1)?.Id ?? 0,
                topicDao.GetByName(topic2)?.Id ?? 0, topicDao.GetByName(topic3)?.Id ?? 0,
                topicDao.GetByName(topic4)?.Id ?? 0, topicDao.GetByName(topic5)?.Id ?? 0));
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