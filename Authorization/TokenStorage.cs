using System.Collections.Concurrent;

namespace scrubsAPI
{
public class TokenStorage
    {
    private ConcurrentDictionary<string, string> _tokens = new ConcurrentDictionary<string, string>();

    public void AddToken(string userId, string token)
    {

        _tokens[token] = userId;


        var timer = new Timer(_ =>
        {
            RemoveToken(userId);
        }, null, TimeSpan.FromMinutes(15), Timeout.InfiniteTimeSpan);
    }


    public bool RemoveToken(string token)
    {

        return _tokens.TryRemove(token, out _);
    }

    public bool TokenExists(string token)
    {
        return _tokens.ContainsKey(token);
    }
}
}
