using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicShare.WebServer;

public class MessageDispatcher
{
    private readonly object _lock = new();
    private readonly List<Action<string>> _actions = new();

    public void DispatchMessage(string message)
    {
        lock (_lock)
        {
            foreach (var action in _actions)
            {
                action(message);
            }
        }
    }

    public void Register(Action<string> action)
    {
        lock (_lock)
        {
            _actions.Add(action);
        }
    }
}