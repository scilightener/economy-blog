using EconomyBlog.Models;

namespace EconomyBlog.ORM;

public interface IUserDao
{
    public IEnumerable<User> GetAll();
    public User? GetById(int id);
    public int Insert(string login, string password);
    public void Remove(int? id);
    public void Update(string field, string value, int? id);
}