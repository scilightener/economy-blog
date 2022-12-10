using System.Data.SqlClient;
using System.Net;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.Models;
using EconomyBlog.ORM;
using EconomyBlog.ServerLogic.SessionLogic;
using static EconomyBlog.Messages;

namespace EconomyBlog.Controllers;

[HttpController("feed")]
public class FeedController : Controller
{
    [HttpPOST("^edit/$")]
    public static ActionResult CreateNewPost(Guid sessionId, string title, string text)
    {
        if (sessionId == Guid.Empty) return new UnauthorizedResult();
        var session = SessionManager.GetSessionInfo(sessionId);
        if (session is null) return new UnauthorizedResult();
        var dao = new PostDao();
        try
        {
            dao.Insert(new Post(title, text, session.Login, DateTime.Now));
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
    //
    // [HttpGET(@"^\d+/$")]
    // public static ActionResult GetPost(Guid sessionId, string path)
    // {
    //     if (sessionId == Guid.Empty || !int.TryParse(path.Split('/')[^2], out var id))
    //         return new ErrorResult(PostNotFound);
    //     var postDao = new PostDao();
    //     Post? post;
    //     try
    //     {
    //         post = postDao.GetById(id);
    //     }
    //     catch (SqlException e)
    //     {
    //         Console.WriteLine(e.Message);
    //         return new ErrorResult(DbError);
    //     }
    //     if (post is null) return new ErrorResult(PostNotFound);
    //     return new ActionResult
    //     {
    //         ContentType = "application/json",
    //         Buffer = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(post))
    //     };
    // }

    [HttpGET("^edit/$")]
    public static ActionResult GetNewPostPage(Guid sessionId, string path) =>
        sessionId == Guid.Empty ? new UnauthorizedResult() : ProcessStatic("feed", path);

    [HttpGET]
    public static ActionResult GetFeedPage(Guid sessionId, string path)
    {
        var dao = new PostDao();
        IEnumerable<Post>? posts;
        try
        {
            posts = dao.GetAll();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }

        return ProcessStatic("feed", path, new { Posts = posts.OrderByDescending(post => post.Date) });
    }
}