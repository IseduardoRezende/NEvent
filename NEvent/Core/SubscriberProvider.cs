using NEvent.Interfaces;
using Microsoft.Extensions.Logging;

namespace NEvent.Core
{
    public class SubscriberProvider : ISubscriberProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SubscriberProvider>? _logger;
      
        public SubscriberProvider(
            IServiceProvider serviceProvider,
            ILogger<SubscriberProvider>? logger = null)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public ISubscriber<TEventArgs> GetSubscriber<TEventArgs>()
            where TEventArgs : EventArgs
        {
            object? result = _serviceProvider.GetService(typeof(ISubscriber<TEventArgs>));
        
            ArgumentNullException.ThrowIfNull(result, nameof(result));

            return (result as ISubscriber<TEventArgs>)!;
        }
    }
}
