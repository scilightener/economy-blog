using EconomyBlog.Models;

namespace EconomyBlog.ORM;

public class UsersFavoriteTopicsDao
{
    private const string TableName = "[dbo].[UsersFavoriteTopics]";
    private const string DbName = "dev_basics_sem1";

    private readonly DataBase _orm;
    public UsersFavoriteTopicsDao() => _orm = new DataBase(DbName, TableName);

    public IEnumerable<UsersFavoriteTopics> GetAll() => _orm.Select<UsersFavoriteTopics>();

    public UsersFavoriteTopics? GetById(int id) =>
        _orm.Select<UsersFavoriteTopics>($"select * from {TableName} where id='{id}'").FirstOrDefault();

    public int Insert(UsersFavoriteTopics usersFavoriteTopics) => _orm.Insert(usersFavoriteTopics);

    public void Delete(UsersFavoriteTopics usersFavoriteTopics) => _orm.Delete(usersFavoriteTopics.Id);

    public void Update(UsersFavoriteTopics usersFavoriteTopics) =>
        _orm.Update(usersFavoriteTopics.Id, usersFavoriteTopics);
}