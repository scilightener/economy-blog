using EconomyBlog.Models;

namespace EconomyBlog.ORM;

public class SessionDao
{
    private const string TableName = "[dbo].[Sessions]";
    private const string DbName = "dev_basics_sem1";

    private readonly DataBase _orm;

    public SessionDao() => _orm = new DataBase(DbName, TableName);
    
    public Session? Select(Guid guid) 
        => _orm.Select<Session>($"select * from {TableName} where guid='{guid.ToString()}'").FirstOrDefault();

    public void Insert(Session session) 
        => _orm.Insert(session);

    public void Delete(Guid guid) => _orm.DeleteWhere("id", guid.ToString());
}