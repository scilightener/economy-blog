using EconomyBlog.Models;

namespace EconomyBlog.ORM;

public class UserDao : IUserDao
{
    private const string TableName = "[dbo].[Users]";
    private const string DbName = "dev_basics_sem1";

    private readonly DataBase _orm; 

    public UserDao()
    {
        _orm = new DataBase(DbName, TableName);
    }

    public IEnumerable<User> GetAll() => _orm.Select<User>();

    public User? GetById(int id) => _orm.Select<User>($"select * from {TableName} where user_id={id}").FirstOrDefault();

    public int Insert(string login, string password) => _orm.Insert(new User(login, password));

    public void Remove(int? id = null) => _orm.Delete(id);

    public void Update(string field, string value, int? id = null) => _orm.Update(field, value, id);

    public void Update(int id, User user) => _orm.Update(id, user);
}