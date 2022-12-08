using EconomyBlog.Models;

namespace EconomyBlog.ORM;

public class PostDao
{
    private const string TableName = "[dbo].[Posts]";
    private const string DbName = "dev_basics_sem1";

    private readonly DataBase _orm;
    public PostDao() => _orm = new DataBase(DbName, TableName);

    public IEnumerable<Post> GetAll() => _orm.Select<Post>();
    
    public Post? GetById(int id) => _orm.Select<Post>($"select * from {TableName} where id='{id}'").FirstOrDefault();

    public int Insert(Post post) => _orm.Insert(post);

    public void Delete(Post post) => _orm.Delete(post.Id);
}