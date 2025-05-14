namespace NEvent.Interfaces
{
    public interface IEventAggregator
    {
        bool TrySubscribe<TEventArgs>() where TEventArgs : EventArgs;

        bool TrySubscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler)
            where TEventArgs : EventArgs;

        bool TryUnSubscribe<TEventArgs>() where TEventArgs : EventArgs;

        bool TryUnSubscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler)
            where TEventArgs : EventArgs;

        Task PublishAsync<TEventArgs>(object sender, TEventArgs args, CancellationToken cancellationToken = default)
            where TEventArgs : EventArgs;
    }
}
