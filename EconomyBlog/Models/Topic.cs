using EconomyBlog.Attributes;

namespace EconomyBlog.Models;

public class Topic
{
    public int Id { get; }
    [DbItem("name")] public string Name { get; }
    [DbItem("description")] public string Description { get; }

    public Topic(int id, string name, string description) : this(name, description) => Id = id;

    public Topic(string name, string description)
    {
        Name = name;
        Description = description;
    }
}