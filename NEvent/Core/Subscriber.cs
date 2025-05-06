using NEvent.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace NEvent.Core
{
    public sealed class Subscriber<TEventArgs> : ISubscriber<TEventArgs>
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

        public bool TryAddOrUpdate(IEventHandler<TEventArgs> eventHandler)
        {
            lock (_lock)
            {
                ArgumentNullException.ThrowIfNull(eventHandler, nameof(eventHandler));

                Type key = typeof(TEventArgs);

                TryGetValues(key, out List<IEventHandler<TEventArgs>>? eventHandlers);

                if (eventHandlers is null)
                    return _subscribers.TryAdd(key, [eventHandler]);

                List<IEventHandler<TEventArgs>> updatedEventHandlers = [.. eventHandlers, eventHandler];

                return _subscribers.TryUpdate(key, updatedEventHandlers, eventHandlers);
            }
        }

        public bool TryRemove(IEventHandler<TEventArgs> eventHandler)
        {
            lock (_lock)
            {
                ArgumentNullException.ThrowIfNull(eventHandler, nameof(eventHandler));

                if (!TryGetValues(typeof(TEventArgs), out List<IEventHandler<TEventArgs>>? eventHandlers))
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

        public bool TryGetValues(Type type, out List<IEventHandler<TEventArgs>>? eventHandlers)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));

            return _subscribers.TryGetValue(type, out eventHandlers);
        }
    }
}
