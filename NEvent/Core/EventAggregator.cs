using System.Reflection;
using NEvent.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace NEvent.Core
{
    public sealed class EventAggregator : IEventAggregator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISubscriberProvider _subscriberProvider;
        private readonly IEventFilterProvider _eventFilterProvider;

        public EventAggregator(
            IServiceProvider serviceProvider,
            ISubscriberProvider subscriberProvider,
            IEventFilterProvider eventFilterProvider)
        {
            _serviceProvider = serviceProvider;
            _subscriberProvider = subscriberProvider;
            _eventFilterProvider = eventFilterProvider;
        }

        public bool TrySubscribe<TEventArgs>() where TEventArgs : EventArgs
        {
            IEnumerable<IEventHandler<TEventArgs>> eventHandlers = _serviceProvider.GetServices<IEventHandler<TEventArgs>>();

            foreach (IEventHandler<TEventArgs> eventHandler in eventHandlers) 
            {
                if (!TrySubscribe(eventHandler))
                    return false;
            }            

            return true;
        }

        public bool TrySubscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler)
            where TEventArgs : EventArgs
        {
            ArgumentNullException.ThrowIfNull(eventHandler, nameof(eventHandler));

            ISubscriber<TEventArgs> subscriber = _subscriberProvider.Get<TEventArgs>();
            return subscriber.TryAddOrUpdate(eventHandler);
        }

        public bool TryUnSubscribe<TEventArgs>() where TEventArgs : EventArgs
        {
            IEnumerable<IEventHandler<TEventArgs>> eventHandlers = _serviceProvider.GetServices<IEventHandler<TEventArgs>>();

            foreach (IEventHandler<TEventArgs> eventHandler in eventHandlers)
            {
                if (!TryUnSubscribe(eventHandler))
                    return false;
            }

            return true;
        }

        public bool TryUnSubscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler)
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
            IEnumerable<IEventFilter<TEventArgs>>? eventFilters = _eventFilterProvider.GetAll<TEventArgs>();

            if (!CanPublish(subscriber, out List<IEventHandler<TEventArgs>> eventHandlers))
                return;

            if (eventFilters is null || !eventFilters.Any())
            {
                eventHandlers = ApplyEventOrder(eventHandlers);               
                await ExecuteHandlersAsync(sender, args, eventHandlers, cancellationToken);            
                return;
            }
            
            eventFilters = ApplyEventOrder(eventFilters);
            eventHandlers = ApplyEventOrder(eventHandlers);            
            await ExecuteFiltersAndHandlersAsync(sender, args, eventFilters, eventHandlers, cancellationToken);
        }

        private static async Task ExecuteFiltersAndHandlersAsync<TEventArgs>(
            object sender,
            TEventArgs args,
            IEnumerable<IEventFilter<TEventArgs>> eventFilters,
            List<IEventHandler<TEventArgs>> eventHandlers,
            CancellationToken cancellationToken) where TEventArgs : EventArgs
        {
            ArgumentNullException.ThrowIfNull(args, nameof(args));
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(eventFilters, nameof(eventFilters));
            ArgumentNullException.ThrowIfNull(eventHandlers, nameof(eventHandlers));

            foreach (IEventFilter<TEventArgs> eventFilter in eventFilters)
            {
                EventFilterResult filterResult = await eventFilter.OnBeforePublishAsync(sender, args, cancellationToken);

                if (filterResult is EventFilterResult.CancelCompletely)
                    break;

                if (filterResult is EventFilterResult.Skip)
                    continue;

                if (filterResult is not EventFilterResult.SkipHandlers)
                {
                    await ExecuteHandlersAsync(sender, args, eventHandlers, cancellationToken);
                }

                await eventFilter.OnAfterPublishAsync(sender, args, cancellationToken);
            }
        }

        private static async Task ExecuteHandlersAsync<TEventArgs>(
            object sender,
            TEventArgs args,
            List<IEventHandler<TEventArgs>> eventHandlers,
            CancellationToken cancellationToken) where TEventArgs : EventArgs
        {
            ArgumentNullException.ThrowIfNull(args, nameof(args));
            ArgumentNullException.ThrowIfNull(sender, nameof(sender));
            ArgumentNullException.ThrowIfNull(eventHandlers, nameof(eventHandlers));

            foreach (IEventHandler<TEventArgs> eventHandler in eventHandlers)
            {
                await eventHandler.HandleAsync(sender, args, cancellationToken);
            }
        }

        private static bool CanPublish<TEventArgs>(ISubscriber<TEventArgs> subscriber, out List<IEventHandler<TEventArgs>> eventHandlers)
            where TEventArgs : EventArgs
        {
            ArgumentNullException.ThrowIfNull(subscriber, nameof(subscriber));

            return subscriber.TryGetValues(typeof(TEventArgs), out eventHandlers!) && eventHandlers is { Count: > 0 };
        }

        private static List<T> ApplyEventOrder<T>(IEnumerable<T> eventItems)     
            where T : notnull
        {
            ArgumentNullException.ThrowIfNull(eventItems, nameof(eventItems));

            return [.. eventItems.Select(e =>
            {
                Type type = e.GetType();
                EventOrderAttribute? attribute = type.GetCustomAttribute<EventOrderAttribute>();
                int order = attribute?.Value ?? int.MaxValue;
                return new { EventItem = e, Order = order };
            })
           .OrderBy(f => f.Order)
           .Select(f => f.EventItem)];
        }        
    }
}
