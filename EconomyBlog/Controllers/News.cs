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

[HttpController("news")]
public class NewsController : Controller
{
    [HttpPOST("^edit/$")]
    public static ActionResult PostNews(Guid sessionId, string title, string text, int topic1, int topic2, int topic3)
    {
        if (sessionId == Guid.Empty) return new UnauthorizedResult();
        var session = SessionManager.GetSessionInfo(sessionId);
        if (session is null) return new UnauthorizedResult();
        if (session.Login != "scilightener") return new UnauthorizedResult(NotAnAdmin);
        var dao = new NewsDao();
        try
        {
            dao.Insert(new News(title, text, DateTime.Now, topic1+1, topic2+1, topic3+1));
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

    [HttpGET(@"^\d+/$")]
    public static ActionResult GetPost(Guid sessionId, string path)
    {
        if (sessionId == Guid.Empty || !int.TryParse(path.Split('/')[^2], out var id))
            return new ErrorResult(PostNotFound);
        var dao = new NewsDao();
        News? news;
        try
        {
            news = dao.GetById(id);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }
        if (news is null) return new ErrorResult(PostNotFound);
        return new ActionResult
        {
            ContentType = "application/json",
            Buffer = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(news))
        };
    }
    
    [HttpGET("^edit/$")]
    public static ActionResult GetNewPostPage(Guid sessionId, string path)
    {
        if (sessionId == Guid.Empty) return new UnauthorizedResult();
        var session = SessionManager.GetSessionInfo(sessionId);
        if (session is null) return new UnauthorizedResult();
        if (session.Login != "scilightener")
            return new UnauthorizedResult(NotAnAdmin);
        IEnumerable<Topic>? topics;
        try
        {
            topics = new TopicsDao().GetAll();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }
        return ProcessStatic("news", path, new { Topics = topics });
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

        var isAdmin = session.Login == "scilightener";
        return ProcessStatic("news", path,
            new { News = news.OrderByDescending(newsItem => newsItem.Date), IsAdmin = isAdmin });
    }
}