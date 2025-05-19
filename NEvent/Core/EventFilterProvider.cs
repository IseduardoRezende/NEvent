using NEvent.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace NEvent.Core
{
    public sealed class EventFilterProvider(IServiceProvider serviceProvider) : IEventFilterProvider
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public IEnumerable<IEventFilter<TEventArgs>> GetAll<TEventArgs>() where TEventArgs : EventArgs
        {
            object? results = _serviceProvider.GetServices<IEventFilter<TEventArgs>>();

            ArgumentNullException.ThrowIfNull(results, nameof(results));

            return (results as IEnumerable<IEventFilter<TEventArgs>>)!;
        }
    }
}
