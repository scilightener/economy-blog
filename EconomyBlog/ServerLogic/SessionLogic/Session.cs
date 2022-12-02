namespace EconomyBlog.ServerLogic.SessionLogic;

public class Session
{
    public readonly Guid Id;
    public readonly int UserId;
    public readonly string Login;
    public readonly DateTime CreateDateTime;

    public Session(Guid id, int userId, string login, DateTime created)
    {
        Id = id;
        UserId = userId;
        Login = login;
        CreateDateTime = created;
    }
}