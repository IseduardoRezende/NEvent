namespace NEvent.Interfaces
{
    public interface ISubscriber<TEventArgs> where TEventArgs : EventArgs
    {        
        bool TryAddOrUpdate(IEventHandler<TEventArgs> eventHandler);

        bool TryRemove(IEventHandler<TEventArgs> eventHandler);

        bool TryGetValues(Type type, out List<IEventHandler<TEventArgs>>? eventHandlers);
    }
}
