using EconomyBlog.Attributes;

namespace EconomyBlog.Models;

public class Session
{
    [DbItem("guid")] public string SessionId => Id.ToString();
    [DbItem("id")] public int UserId { get; }
    [DbItem("login")] public string Login { get; }
    [DbItem("created_at")] public string SqlDateTime => CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss");

    public readonly Guid Id;
    public readonly DateTime CreateDateTime;


    public Session(string guidString, int userId, string login, DateTime created) : this(
        Guid.TryParse(guidString, out var parsed) ? parsed : Guid.Empty, userId, login, created)
    {
    }

    public Session(Guid guid, int userId, string login, DateTime created)
    {
        Id = guid;
        UserId = userId;
        Login = login;
        CreateDateTime = created;
    }
}