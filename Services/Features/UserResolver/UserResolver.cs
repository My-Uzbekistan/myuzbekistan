namespace myuzbekistan.Services;

public class UserResolver
{
    private ConcurrentDictionary<string, long> UserIds { get; } = new();

    public bool TryGetUserId(string sessionId, out long userId)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            throw new ArgumentException("Session ID cannot be null or empty.", nameof(sessionId));
        }
        return UserIds.TryGetValue(sessionId, out userId);
    }

    public void SetUserId(long userId, string sessionId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
        }
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            throw new ArgumentException("Session ID cannot be null or empty.", nameof(sessionId));
        }

        if (UserIds.ContainsKey(sessionId))
        {
            UserIds[sessionId] = userId;
        }
        else
        {
            UserIds.TryAdd(sessionId, userId);
        }
    }
}