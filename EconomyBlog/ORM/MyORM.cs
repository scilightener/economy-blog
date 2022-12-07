using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using EconomyBlog.Attributes;

namespace EconomyBlog.ORM;

internal class DataBase
{
    private readonly string _connectionString;
    private readonly string _tableName;

    public DataBase(string dbName, string tableName)
    {
        _tableName = tableName;
        _connectionString =
            @$"Data Source=DESKTOP-MFCEQVI\SQLEXPRESS;Initial Catalog={dbName};Integrated Security=True";
    }

    public IEnumerable<T> Select<T>(string? query = null) where T : class
    {
        query ??= $"select * from {_tableName}";
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var cmd = new SqlCommand(query, connection);
        using var reader = cmd.ExecuteReader();

        if (!reader.HasRows || !reader.Read()) yield break;

        var ctor = GetConstructor<T>(reader);
        if (ctor is null) yield break;

        var parameters = new object[reader.FieldCount];
        reader.GetValues(parameters);
        yield return (T)ctor.Invoke(parameters);
        while (reader.Read())
        {
            reader.GetValues(parameters);
            yield return (T)ctor.Invoke(parameters);
        }
    }

    public int Insert<T>(T instance)
    {
        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(p => /*p.GetValue(instance) is not null && */p.GetCustomAttribute(typeof(DbItem)) is not null)
            .ToDictionary(p => (p.GetCustomAttribute(typeof(DbItem)) as DbItem)!.Name,
                p => $"'{p.GetValue(instance) ?? string.Empty}'");
        var query =
            $"insert into {_tableName} ({string.Join(", ", properties.Keys)}) output inserted.id " +
            $"values ({string.Join(", ", properties.Values)})";
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var cmd = new SqlCommand(query, connection);
        return (int)cmd.ExecuteScalar();
    }

    public void Delete(int? id = null)
    {
        var query = $"delete from {_tableName}";
        query += id is not null ? $"where id={id}" : "";
        Execute(query);
    }

    public void DeleteWhere(string columnName, string value)
    {
        var query = $"delete from {_tableName} where {columnName}='{value}'";
        Execute(query);
    }

    public void Update<T>(int id, T instance)
    {
        var changes = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetValue(instance) is not null && p.GetCustomAttribute(typeof(DbItem)) is not null)
            .Select(p =>
                $"{(p.GetCustomAttribute(typeof(DbItem)) as DbItem)!.Name} = '{(p.GetValue(instance)?.ToString() ?? string.Empty).Replace("'", "''")}'");

        var sqlExpression = $"update {_tableName} set {string.Join(',', changes)} where id={id}";

        Execute(sqlExpression);
    }
    
    public void Update(string field, string value, int? id = null)
    {
        var query = $"update {_tableName} set {field}='{value}'";
        query += id is not null ? $" where id={id}" : "";
        Execute(query);
    }

    private void Execute(string query)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var cmd = new SqlCommand(query, connection);
        cmd.ExecuteNonQuery();
    }

    private static ConstructorInfo? GetConstructor<T>(IDataRecord reader) =>
        typeof(T).GetConstructor(
            Enumerable.Range(0, reader.FieldCount)
                .Select(reader.GetFieldType)
                .ToArray());
}