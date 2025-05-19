using NEvent.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace NEvent.Core
{
    public sealed class EventFilterProvider(IServiceProvider serviceProvider) : IEventFilterProvider
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public IEnumerable<IEventFilter<TEventArgs>>? GetAll<TEventArgs>() where TEventArgs : EventArgs
        {
            return _serviceProvider.GetServices<IEventFilter<TEventArgs>>();
        }
    }
}
