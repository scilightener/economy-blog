using EconomyBlog.Attributes;

namespace EconomyBlog.Models;

public class Post
{
    public int Id { get; }
    [DbItem("title")] public string Title { get; }
    [DbItem("text")] public string Text { get; }
    [DbItem("author")] public string Author { get; }
    [DbItem("publication_date")] public string SqlDateTime => Date.ToString("yyyy-MM-dd HH:mm:ss");

    public readonly DateTime Date;
    
    public Post(int id, string title, string text, string author, DateTime date)
    {
        Id = id;
        Title = title;
        Text = text;
        Author = author;
        Date = date;
    }

    public Post(string title, string text, string author, DateTime date)
    {
        Title = title;
        Text = text;
        Author = author;
        Date = date;
    }
}