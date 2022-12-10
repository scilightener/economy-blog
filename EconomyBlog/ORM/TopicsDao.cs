namespace EconomyBlog.ORM;

public class TopicsDao
{
    private const string TableName = "[dbo].[Topics]";
    private const string DbName = "dev_basics_sem1";

    private readonly DataBase _orm;
    public TopicsDao() => _orm = new DataBase(DbName, TableName);

    public IEnumerable<Topic> GetAll() => _orm.Select<Topic>();

    public Topic? GetById(int id) => _orm.Select<Topic>($"select * from {TableName} where id='{id}'").FirstOrDefault();

    public int Insert(Topic topic) => _orm.Insert(topic);

    public void Delete(Topic topic) => _orm.Delete(topic.Id);

    public Topic? GetByName(string name) =>
        _orm.Select<Topic>($"select * from {TableName} where name='{name}'").FirstOrDefault();
}