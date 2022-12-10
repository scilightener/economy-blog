using System.Data.SqlClient;
using System.Net;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.Models;
using EconomyBlog.ORM;
using EconomyBlog.ServerLogic.SessionLogic;
using static EconomyBlog.Messages;

namespace EconomyBlog.Controllers;

[HttpController("news")]
public class NewsController : Controller
{
    [HttpPOST("^edit/$")]
    public static ActionResult PostNews(Guid sessionId, string title, string text, int topic1 = 0, int topic2 = 0,
        int topic3 = 0)
    {
        if (sessionId == Guid.Empty) return new UnauthorizedResult();
        var session = SessionManager.GetSessionInfo(sessionId);
        if (session is null) return new UnauthorizedResult();
        if (session.Login != "scilightener") return new UnauthorizedResult(NotAnAdmin);
        var dao = new NewsDao();
        try
        {
            dao.Insert(new News(title, text, DateTime.Now, topic1, topic2, topic3));
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
            return new ErrorResult(DbError);
        }

        return new ActionResult
        {
            StatusCode = HttpStatusCode.Redirect,
            RedirectUrl = "../"
        };
    }

    [HttpGET("^sort/$")]
    public static ActionResult GetInterestingNews(Guid sessionId, string path)
    {
        if (sessionId == Guid.Empty) return new UnauthorizedResult();
        var session = SessionManager.GetSessionInfo(sessionId);
        if (session is null) return new UnauthorizedResult();
        User? user;
        IEnumerable<News>? news;
        try
        {
            user = new UserDao().GetById(session.UserId);
            news = new NewsDao().GetAll();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }

        if (user is null) return new ErrorResult(UserNotFound);
        var userTopics = user.FavoriteTopics.Where(topic => topic.Name != "None").Select(topic => topic.Id);
        return ProcessStatic("news", "sorted_news.html",
            new
            {
                News = news.Where(newsItem => newsItem.Topics.Select(topic => topic.Id).Intersect(userTopics).Any())
                    .OrderByDescending(newsItem => newsItem.Date)
            });
    }

    //
    // [HttpGET(@"^\d+/$")]
    // public static ActionResult GetPost(Guid sessionId, string path)
    // {
    //     if (sessionId == Guid.Empty || !int.TryParse(path.Split('/')[^2], out var id))
    //         return new ErrorResult(PostNotFound);
    //     var dao = new NewsDao();
    //     News? news;
    //     try
    //     {
    //         news = dao.GetById(id);
    //     }
    //     catch (SqlException e)
    //     {
    //         Console.WriteLine(e.Message);
    //         return new ErrorResult(DbError);
    //     }
    //     if (news is null) return new ErrorResult(PostNotFound);
    //     return new ActionResult
    //     {
    //         ContentType = "application/json",
    //         Buffer = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(news))
    //     };
    // }

    [HttpGET("^edit/$")]
    public static ActionResult GetNewPostPage(Guid sessionId, string path)
    {
        if (sessionId == Guid.Empty) return new UnauthorizedResult();
        var session = SessionManager.GetSessionInfo(sessionId);
        if (session is null) return new UnauthorizedResult();
        if (Admins.Logins.Contains(session.Login))
            return new UnauthorizedResult(NotAnAdmin);
        IEnumerable<Topic>? topics;
        try
        {
            topics = new TopicsDao().GetAll().Skip(1);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }

        return ProcessStatic("news", path, new { Topics = topics.OrderBy(topic => topic.Name) });
    }

    [HttpGET]
    public static ActionResult GetNewsPage(Guid sessionId, string path)
    {
        if (sessionId == Guid.Empty) return new UnauthorizedResult();
        var session = SessionManager.GetSessionInfo(sessionId);
        if (session is null) return new UnauthorizedResult();
        IEnumerable<News>? news;
        try
        {
            news = new NewsDao().GetAll();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }

        return ProcessStatic("news", path,
            new
            {
                News = news.OrderByDescending(newsItem => newsItem.Date),
                IsAdmin = Admins.Logins.Contains(session.Login)
            });
    }
}