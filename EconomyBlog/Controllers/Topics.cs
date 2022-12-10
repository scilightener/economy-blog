using System.Text;
using System.Text.Json;

namespace EconomyBlog.Controllers;

[HttpController("topics")]
public class TopicsController : Controller
{
    // TODO: сделать страницами, а не json
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