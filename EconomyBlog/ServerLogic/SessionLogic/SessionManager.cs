using Microsoft.Extensions.Caching.Memory;

namespace EconomyBlog.ServerLogic.SessionLogic;

public static class SessionManager
{
    private static readonly MemoryCache Cache = new(new MemoryCacheOptions());

    public static Guid CreateSession(int accountId, string login, DateTime created)
    {
        var session = new Session(Guid.NewGuid(), accountId, login, created);
        var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
        Cache.Set(session.Id, session, cacheOptions);
        return session.Id;
    }

    public static bool CheckSession(Guid id) => Cache.TryGetValue(id, out _);

    public static Session? GetSessionInfo(Guid id) => CheckSession(id) ? Cache.Get(id) as Session : null;
}