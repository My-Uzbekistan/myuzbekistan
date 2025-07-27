using System.Collections.Concurrent;

namespace Server.Infrastructure;
public class Debouncer
{
    private readonly int debounceInterval;
    private readonly ConcurrentDictionary<string, Timer> timers = new();

    public Debouncer(int debounceInterval)
    {
        this.debounceInterval = debounceInterval;
    }

    public void Debounce(string key, Action action)
    {
        timers.AddOrUpdate(
            key,
            // Key doesn't exist -> run action and create timer
            k =>
            {
                action(); // Execute action immediately for first call
                return CreateTimer(k);
            },
            // Key exists -> just reset timer
            (k, existingTimer) =>
            {
                existingTimer.Change(debounceInterval, Timeout.Infinite);
                return existingTimer;
            });
    }

    private Timer CreateTimer(string key)
    {
        return new Timer(_ =>
        {
            timers.TryRemove(key, out var _);
        }, null, debounceInterval, Timeout.Infinite);
    }
}
