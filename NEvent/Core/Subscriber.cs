using NEvent.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace NEvent.Core
{
    public class Subscriber<TEventArgs> : ISubscriber<TEventArgs>
        where TEventArgs : EventArgs
    {
        private readonly ConcurrentDictionary<Type, List<IEventHandler<TEventArgs>>>
            _subscribers = [];

        private readonly ILogger<Subscriber<TEventArgs>>? _logger;
        private readonly Lock _lock = new();

        public Subscriber(ILogger<Subscriber<TEventArgs>>? logger = null)
        {
            _logger = logger;
        }

        public bool TryUpdate(List<IEventHandler<TEventArgs>> eventHandlers)
        {
            ArgumentNullException.ThrowIfNull(eventHandlers, nameof(eventHandlers));

            Type key = typeof(TEventArgs);
            return _subscribers.TryUpdate(key, eventHandlers, _subscribers[key]);
        }

        public bool TryAdd(List<IEventHandler<TEventArgs>> eventHandlers)
        {
            ArgumentNullException.ThrowIfNull(eventHandlers, nameof(eventHandlers));

            return _subscribers.TryAdd(typeof(TEventArgs), eventHandlers);
        }

        public bool TryRemove(IEventHandler<TEventArgs> eventHandler)
        {
            lock (_lock)
            {
                ArgumentNullException.ThrowIfNull(eventHandler, nameof(eventHandler));

                if (!TryGetValue(typeof(TEventArgs), out List<IEventHandler<TEventArgs>>? eventHandlers))
                    return false;

                foreach ((int i, IEventHandler<TEventArgs> handler) in eventHandlers!.Index())
                {
                    if (handler.GetType() == eventHandler.GetType())
                    {
                        eventHandlers!.RemoveAt(i);
                        return true;
                    }
                }

                return false;
            }
        }

        public bool TryGetValue(Type type, out List<IEventHandler<TEventArgs>>? eventHandlers)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            return _subscribers.TryGetValue(type, out eventHandlers);
        }
    }
}
