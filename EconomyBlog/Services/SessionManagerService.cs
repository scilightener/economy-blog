using EconomyBlog.Models;
using EconomyBlog.ORM;
using Microsoft.Extensions.Caching.Memory;

namespace EconomyBlog.ServerLogic.SessionLogic;

public static class SessionManager
{
    private static readonly MemoryCache Cache = new(new MemoryCacheOptions());

    public static Guid CreateSession(int userId, string login, DateTime created, bool rememberMe = false)
    {
        var session = new Session(Guid.NewGuid(), userId, login, created);
        var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
        Cache.Set(session.Id, session, cacheOptions);
        if (rememberMe) new SessionDao().Insert(session);
        return session.Id;
    }

    public static bool CheckSession(Guid id)
    {
        var session = new SessionDao().Select(id);
        return Cache.TryGetValue(id, out _) ||
               session is not null && (DateTime.Now - session.CreateDateTime).Days < 150;
    }

    public static Session? GetSessionInfo(Guid id) =>
        Cache.TryGetValue(id, out Session? session) ? session : new SessionDao().Select(id);
}