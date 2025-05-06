namespace NEvent.Interfaces
{
    public interface IEventAggregator
    {
        bool Subscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler)
            where TEventArgs : EventArgs;

        bool UnSubscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler)
            where TEventArgs : EventArgs;

        Task PublishAsync<TEventArgs>(object sender, TEventArgs args, CancellationToken cancellationToken = default)
            where TEventArgs : EventArgs;
    }
}
