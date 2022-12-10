using EconomyBlog.Services;

namespace EconomyBlog.Controllers;

[HttpController("feed")]
public class FeedController : Controller
{
    [HttpGET(@"^edit/\d+/$")]
    public static ActionResult GetEditPostPage(Guid sessionId, string path)
    {
        if (!int.TryParse(path.Split('/')[^2], out var postId))
            return new ErrorResult(PostNotFound);
        if (sessionId == Guid.Empty) return new UnauthorizedResult();
        var session = SessionManager.GetSessionInfo(sessionId);
        if (session is null) return new UnauthorizedResult();
        Post? post;
        try
        {
            post = new PostDao().GetById(postId);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }

        if (post is null) return new ErrorResult(PostNotFound);
        return session.Login != post.Author
            ? new UnauthorizedResult(AnotherUsersPostAccessFail)
            : ProcessStatic("feed", "edit_post.html", new { Post = post, Login = session.Login });
    }

    [HttpPOST(@"^edit/\d+/$")]
    public static ActionResult UpdatePost(Guid sessionId, string title, string text, int postId)
    {
        if (sessionId == Guid.Empty) return new UnauthorizedResult();
        var session = SessionManager.GetSessionInfo(sessionId);
        if (session is null) return new UnauthorizedResult();
        try
        {
            new PostDao().Update(postId, new Post(title, text, session.Login, DateTime.Now));
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }

        return new ActionResult
        {
            StatusCode = HttpStatusCode.Redirect,
            RedirectUrl = "/feed/"
        };
    }
    
    [HttpPOST("^create/$")]
    public static ActionResult CreateNewPost(Guid sessionId, string title, string text)
    {
        if (sessionId == Guid.Empty) return new UnauthorizedResult();
        var session = SessionManager.GetSessionInfo(sessionId);
        if (session is null) return new UnauthorizedResult();
        try
        {
            new PostDao().Insert(new Post(title, text, session.Login, DateTime.Now));
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

    [HttpGET("^create/$")]
    public static ActionResult GetNewPostPage(Guid sessionId, string path) =>
        sessionId == Guid.Empty ? new UnauthorizedResult() : ProcessStatic("feed", path);

    [HttpGET]
    public static ActionResult GetFeedPage(Guid sessionId, string path)
    {
        IEnumerable<Post>? posts;
        try
        {
            posts = new PostDao().GetAll();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }

        return ProcessStatic("feed", path,
            new
            {
                Posts = posts.OrderByDescending(post => post.Date),
                Login = SessionManager.GetSessionInfo(sessionId)?.Login
            });
    }
}