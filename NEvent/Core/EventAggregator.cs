using NEvent.Interfaces;
using Microsoft.Extensions.Logging;

namespace NEvent.Core
{
    public class EventAggregator : IEventAggregator
    {
        private readonly ISubscriberProvider _subscriberService;
        private readonly ILogger<EventAggregator>? _logger;

        public EventAggregator(
            ISubscriberProvider subscriberService, 
            ILogger<EventAggregator>? logger = null)
        {
            _subscriberService = subscriberService;
            _logger = logger;
        }

        public bool Subscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler)
            where TEventArgs : EventArgs
        {
            ArgumentNullException.ThrowIfNull(eventHandler, nameof(eventHandler));

            ISubscriber<TEventArgs> subscriber = _subscriberService.GetSubscriber<TEventArgs>();

            var type = typeof(TEventArgs);

            subscriber.TryGetValue(type, out List<IEventHandler<TEventArgs>>? eventHandlers);

            if (eventHandlers is null)
                return subscriber.TryAdd([eventHandler]);

            return subscriber.TryUpdate(eventHandlers);
        }

        public bool UnSubscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler)
            where TEventArgs : EventArgs
        {
            ArgumentNullException.ThrowIfNull(eventHandler, nameof(eventHandler));

            ISubscriber<TEventArgs> subscriber = _subscriberService.GetSubscriber<TEventArgs>();

            return subscriber.TryRemove(eventHandler);
        }

        public async Task PublishAsync<TEventArgs>(object _, TEventArgs data, CancellationToken cancellationToken = default)
            where TEventArgs : EventArgs
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));

            ISubscriber<TEventArgs> subscriber = _subscriberService.GetSubscriber<TEventArgs>();

            if (!subscriber.TryGetValue(typeof(TEventArgs), out List<IEventHandler<TEventArgs>>? eventHandlers))
                return;

            foreach (var eventHandler in eventHandlers!)
                await eventHandler.HandleAsync(data, cancellationToken);
        }
    }
}
