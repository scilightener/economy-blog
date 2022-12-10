namespace EconomyBlog.ORM;

public class NewsDao
{
    private const string TableName = "[dbo].[News]";
    private const string DbName = "dev_basics_sem1";

    private readonly DataBase _orm;
    public NewsDao() => _orm = new DataBase(DbName, TableName);

    public IEnumerable<News> GetAll() => _orm.Select<News>();

    public News? GetById(int id) => _orm.Select<News>($"select * from {TableName} where id='{id}'").FirstOrDefault();

    public int Insert(News news) => _orm.Insert(news);

    public void Delete(News news) => _orm.Delete(news.Id);
}