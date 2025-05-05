namespace NEvent.Interfaces
{
    public interface ISubscriber<TEventArgs> where TEventArgs : EventArgs
    {
        bool TryUpdate(List<IEventHandler<TEventArgs>> eventHandlers);

        bool TryAdd(List<IEventHandler<TEventArgs>> eventHandlers);

        bool TryRemove(IEventHandler<TEventArgs> eventHandler);

        bool TryGetValue(Type type, out List<IEventHandler<TEventArgs>>? eventHandlers);
    }
}
