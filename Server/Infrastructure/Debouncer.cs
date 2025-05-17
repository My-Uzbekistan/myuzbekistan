using System.Diagnostics;

namespace Server.Infrastructure;
public class Debouncer(int debounceInterval)
{
    private Dictionary<string, Timer> timers = [];

    public void Debounce(string key, Action action)
    {
        if (timers.ContainsKey(key))
        {
            timers[key].Change(debounceInterval, Timeout.Infinite);
        }
        else
        {
            action(); // Немедленное выполнение действия при первом вызове

            Timer timer = new(_ =>
            {
                timers.Remove(key);
                
            }, null, debounceInterval, Timeout.Infinite);

            timers.Add(key, timer);
        }
    }
}