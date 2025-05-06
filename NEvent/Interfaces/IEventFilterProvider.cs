namespace NEvent.Interfaces
{
    public interface IEventFilterProvider
    {
        IEnumerable<IEventFilter<TEventArgs>> GetAll<TEventArgs>() where TEventArgs : EventArgs;
    }
}
