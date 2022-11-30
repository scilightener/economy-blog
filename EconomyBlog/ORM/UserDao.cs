using System.Data.SqlClient;
using EconomyBlog.Models;

namespace EconomyBlog.ORM;

public class UserDao : IUserDao
{
    private const string ConnectionString =
        $@"Data Source=DESKTOP-MFCEQVI\SQLEXPRESS;Initial Catalog={DbName};Integrated Security=True";

    private const string TableName = "[dbo].[Accounts]";
    private const string DbName = "SteamDB";

    public IEnumerable<User> GetAll()
    {
        var query = $"select * from {TableName}";
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();
        
        var cmd = new SqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();
        if (!reader.HasRows) yield break;
        while (reader.Read())
            yield return new User(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetInt32(5),
                reader.GetString(6),
                reader.GetString(7),
                reader.GetDouble(8));
    }

    public User? GetById(int id)
    {
        var query = $"select * from {TableName} where Id={id}";
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();
        
        var cmd = new SqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();
        
        if (!reader.HasRows || !reader.Read()) return null;
        
        return new User(
            reader.GetInt32(0),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetString(4),
            reader.GetInt32(5),
            reader.GetString(6),
            reader.GetString(7),
            reader.GetDouble(8));
    }

    public void Insert(string login, string password)
    {
        var query = $"insert into {TableName} values ('{login}', '{password}')";
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        var cmd = new SqlCommand(query, connection);
        cmd.ExecuteNonQuery();
    }

    public void Remove(int? id = null)
    {
        var query = $"delete from {TableName}";
        query += id is not null ? $"where Id={id}" : "";

        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        var cmd = new SqlCommand(query);
        cmd.ExecuteNonQuery();
    }

    public void Update(string field, string value, int? id = null)
    {
        var query = $"update {TableName} set {field}={value}";
        query += id is not null ? $"where Id={id}" : "";

        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        var cmd = new SqlCommand(query);
        cmd.ExecuteNonQuery();
    }
}