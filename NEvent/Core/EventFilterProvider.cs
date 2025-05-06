using NEvent.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace NEvent.Core
{
    public sealed class EventFilterProvider : IEventFilterProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventFilterProvider>? _logger;

        public EventFilterProvider(
            IServiceProvider serviceProvider,
            ILogger<EventFilterProvider>? logger = null)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public IEnumerable<IEventFilter<TEventArgs>> GetAll<TEventArgs>() where TEventArgs : EventArgs
        {
            object? results = _serviceProvider.GetServices<IEventFilter<TEventArgs>>();

            ArgumentNullException.ThrowIfNull(results, nameof(results));

            return (results as IEnumerable<IEventFilter<TEventArgs>>)!;
        }
    }
}
