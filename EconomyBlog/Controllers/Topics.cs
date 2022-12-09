using System.Data.SqlClient;
using System.Text;
using System.Text.Json;
using EconomyBlog.ActionResults;
using EconomyBlog.Attributes;
using EconomyBlog.Models;
using EconomyBlog.ORM;
using static EconomyBlog.Messages;

namespace EconomyBlog.Controllers;

[HttpController("topics")]
public class TopicsController : Controller
{
    [HttpGET(@"^\w+/$")]
    public static ActionResult GetUserByLogin(Guid sessionId, string path)
    {
        if (sessionId == Guid.Empty)
            return new UnauthorizedResult();
        var topicName = path.Split('/', StringSplitOptions.RemoveEmptyEntries)[^1];
        Topic? topic;
        try
        {
            topic = new TopicsDao().GetByName(topicName);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            return new ErrorResult(DbError);
        }
        if (topic is null) return new ErrorResult(UserNotFound);
        return new ActionResult
        {
            ContentType = "application/json",
            Buffer = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(topic))
        };
    }
}