using NEvent.Interfaces;

namespace NEvent.Core
{
    public sealed class SubscriberProvider(IServiceProvider serviceProvider) : ISubscriberProvider
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public ISubscriber<TEventArgs> Get<TEventArgs>()
            where TEventArgs : EventArgs
        {
            object? result = _serviceProvider.GetService(typeof(ISubscriber<TEventArgs>));
        
            ArgumentNullException.ThrowIfNull(result, nameof(result));

            return (result as ISubscriber<TEventArgs>)!;
        }
    }
}
