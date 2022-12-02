using EconomyBlog.Attributes;

namespace EconomyBlog.Models;

public class Post
{
    public int Id { get; }
    [DbItem("title")]
    public string Title { get; }
    [DbItem("text")]
    public string Content { get; }
    [DbItem("author_id")]
    public int Author { get; }
    [DbItem("publication_date")]
    public DateTime Date { get; }
    
    
    public Post(int id, string title, string content, int author, DateTime date)
    {
        Id = id;
        Title = title;
        Content = content;
        Author = author;
        Date = date;
    }

    public Post(string title, string content, int author, DateTime date)
    {
        Title = title;
        Content = content;
        Author = author;
        Date = date;
    }
}