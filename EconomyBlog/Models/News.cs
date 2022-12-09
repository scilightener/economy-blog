using System.Data.SqlClient;
using EconomyBlog.Attributes;
using EconomyBlog.ORM;

namespace EconomyBlog.Models;

public class News
{
    public int Id { get; }
    [DbItem("title")] public string Title { get; }
    [DbItem("text")] public string Text { get; }
    [DbItem("topic1")] public int Topic1 { get; }
    [DbItem("topic2")] public int Topic2 { get; }
    [DbItem("topic3")] public int Topic3 { get; }
    [DbItem("publication_date")] public string SqlDateTime => Date.ToString("yyyy-MM-dd HH:mm:ss");

    public readonly DateTime Date;
    public readonly List<Topic> Topics = new();
    
    // TODO: do smth with the case in which not exactly 3 topics were selected
    public News(int id, string title, string text, DateTime date, int topic1, int topic2, int topic3) : this(title,
        text, date, topic1, topic2, topic3) => Id = id;

    public News(string title, string text, DateTime date, int topic1, int topic2, int topic3)
    {
        Title = title;
        Text = text;
        Date = date;
        Topic1 = topic1;
        Topic2 = topic2;
        Topic3 = topic3;
        var dao = new TopicsDao();
        try
        {
            Topics.Add(dao.GetById(topic1) ?? throw new Exception("Wrong topic1"));
            Topics.Add(dao.GetById(topic2) ?? throw new Exception("Wrong topic2"));
            Topics.Add(dao.GetById(topic3) ?? throw new Exception("Wrong topic3"));
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}