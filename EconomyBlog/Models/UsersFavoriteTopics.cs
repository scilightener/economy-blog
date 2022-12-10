namespace EconomyBlog.Models;

public class UsersFavoriteTopics
{
    public int Id { get; }
    [DbItem("topic1")] public int Topic1 { get; }
    [DbItem("topic2")] public int Topic2 { get; }
    [DbItem("topic3")] public int Topic3 { get; }
    [DbItem("topic4")] public int Topic4 { get; }
    [DbItem("topic5")] public int Topic5 { get; }

    public readonly List<Topic> FavoriteTopics = new();

    public UsersFavoriteTopics(int id, int topic1, int topic2, int topic3, int topic4, int topic5) : this(topic1,
        topic2, topic3, topic4, topic5) =>
        Id = id;

    public UsersFavoriteTopics(int topic1, int topic2, int topic3, int topic4, int topic5)
    {
        Topic1 = topic1;
        Topic2 = topic2;
        Topic3 = topic3;
        Topic4 = topic4;
        Topic5 = topic5;

        try
        {
            var dao = new TopicsDao();
            FavoriteTopics.Add(dao.GetById(topic1) ?? throw new Exception("Wrong topic1"));
            FavoriteTopics.Add(dao.GetById(topic2) ?? throw new Exception("Wrong topic2"));
            FavoriteTopics.Add(dao.GetById(topic3) ?? throw new Exception("Wrong topic3"));
            FavoriteTopics.Add(dao.GetById(topic4) ?? throw new Exception("Wrong topic4"));
            FavoriteTopics.Add(dao.GetById(topic5) ?? throw new Exception("Wrong topic5"));
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}