using NEvent.Interfaces;
using Microsoft.Extensions.Logging;

namespace NEvent.Core
{
    public sealed class EventAggregator : IEventAggregator
    {
        private readonly ISubscriberProvider _subscriberProvider;
        private readonly IEventFilterProvider _eventFilterProvider;
        private readonly ILogger<EventAggregator>? _logger;

        public EventAggregator(
            ISubscriberProvider subscriberProvider,
            IEventFilterProvider eventFilterProvider,
            ILogger<EventAggregator>? logger = null)
        {
            _subscriberProvider = subscriberProvider;
            _eventFilterProvider = eventFilterProvider;
            _logger = logger;
        }

        public bool Subscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler)
            where TEventArgs : EventArgs
        {
            ArgumentNullException.ThrowIfNull(eventHandler, nameof(eventHandler));

            ISubscriber<TEventArgs> subscriber = _subscriberProvider.Get<TEventArgs>();
            return subscriber.TryAddOrUpdate(eventHandler);
        }

        public bool UnSubscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler)
            where TEventArgs : EventArgs
        {
            ArgumentNullException.ThrowIfNull(eventHandler, nameof(eventHandler));

            ISubscriber<TEventArgs> subscriber = _subscriberProvider.Get<TEventArgs>();
            return subscriber.TryRemove(eventHandler);
        }

        public async Task PublishAsync<TEventArgs>(object sender, TEventArgs args, CancellationToken cancellationToken = default)
            where TEventArgs : EventArgs
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(args, nameof(args));

            ISubscriber<TEventArgs> subscriber = _subscriberProvider.Get<TEventArgs>();
            IEnumerable<IEventFilter<TEventArgs>> eventFilters = _eventFilterProvider.GetAll<TEventArgs>();

            if (!CanPublish(subscriber, out List<IEventHandler<TEventArgs>>? eventHandlers))
                return;

            if (!eventFilters.Any())
            {
                await ExecuteHandlersAsync(sender, args, eventHandlers!, cancellationToken);
                return;
            }

            foreach (IEventFilter<TEventArgs> eventFilter in eventFilters)
            {
                EventFilterResult filterResult = await eventFilter.OnBeforePublishAsync(sender, args, cancellationToken);

                if (filterResult is EventFilterResult.CancelCompletely)
                    break;

                if (filterResult is EventFilterResult.Skip)
                    continue;

                if (filterResult is not EventFilterResult.SkipHandlers)
                {
                    await ExecuteHandlersAsync(sender, args, eventHandlers!, cancellationToken);
                }

                await eventFilter.OnAfterPublishAsync(sender, args, cancellationToken);
            }
        }       

        private async Task ExecuteHandlersAsync<TEventArgs>(
            object sender, 
            TEventArgs args, 
            List<IEventHandler<TEventArgs>> eventHandlers, 
            CancellationToken cancellationToken) where TEventArgs : EventArgs
        {
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(args, nameof(args));
            ArgumentNullException.ThrowIfNull(eventHandlers, nameof(eventHandlers));

            foreach (IEventHandler<TEventArgs> eventHandler in eventHandlers)
            {
                await eventHandler.HandleAsync(sender, args, cancellationToken);
            }
        }

        private bool CanPublish<TEventArgs>(ISubscriber<TEventArgs> subscriber, out List<IEventHandler<TEventArgs>>? eventHandlers) 
            where TEventArgs : EventArgs
        {
            return subscriber.TryGetValues(typeof(TEventArgs), out eventHandlers) && eventHandlers is { Count: > 0 };
        }
    }
}
