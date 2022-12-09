using System.Data.SqlClient;
using EconomyBlog.Attributes;
using EconomyBlog.ORM;

namespace EconomyBlog.Models;

public class User
{
    public int Id { get; }
    [DbItem("login")]
    public string? Login { get; }
    [DbItem("password")]
    public string? Password { get; }
    [DbItem("first_name")]
    public string? FirstName { get; }
    [DbItem("last_name")]
    public string? LastName { get; }
    [DbItem("age")]
    public int? Age { get; }
    [DbItem("education")]
    public string? Education { get; }
    [DbItem("job")]
    public string? Job { get; }
    [DbItem("risk_index")]
    public int? RiskIndex { get; }
    [DbItem("favorite_topics")]
    public int FavoriteTopicsId { get; }

    public List<Topic> FavoriteTopics;

    public User(int id, string? login, string? password, string? firstName, string? lastName, int? age, string? education, string? job, int? riskIndex, int favoriteTopicsId)
    {
        Id = id;
        Login = login;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Education = education;
        Job = job;
        RiskIndex = riskIndex;
        FavoriteTopicsId = favoriteTopicsId;

        try
        {
            FavoriteTopics = new UsersFavoriteTopicsDao().GetById(favoriteTopicsId)?.FavoriteTopics ??
                             new List<Topic>();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public User(string login, string password)
    {
        Login = login;
        Password = password;
    }
}