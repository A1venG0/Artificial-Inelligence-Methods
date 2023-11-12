using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class Mediator
    {
        private static Mediator _instance;
        public static Mediator Instance => _instance ?? (_instance = new Mediator());

        public readonly Dictionary<string, List<object>> _messageHandlers = new Dictionary<string, List<object>>();

        public void Register(string message, Action<object> callback)
        {
            if (!_messageHandlers.ContainsKey(message))
                _messageHandlers.Add(message, new List<object>());

            _messageHandlers[message].Add(callback);
        }

        public void Register(string message, Func<object, object> callback)
        {
            if (!_messageHandlers.ContainsKey(message))
                _messageHandlers.Add(message, new List<object>());

            _messageHandlers[message].Add(callback);
        }

        public void Unregister(string message, Action<object> callback)
        {
            if (_messageHandlers.ContainsKey(message))
                _messageHandlers[message].Remove(callback);
        }

        public void Unregister(string message, Func<object, object> callback)
        {
            if (_messageHandlers.ContainsKey(message))
                _messageHandlers[message].Remove(callback);
        }

        public object Send(string message, object args = null)
        {
            if (_messageHandlers.ContainsKey(message))
            {
                foreach (var handler in _messageHandlers[message])
                {
                    if (handler is Action<object> actionHandler)
                        actionHandler(args);
                    else if (handler is Func<object, object> funcHandler)
                        return funcHandler(args);
                }
            }
            return null;
        }
    }
}
