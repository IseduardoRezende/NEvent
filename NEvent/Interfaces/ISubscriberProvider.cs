namespace NEvent.Interfaces
{
    public interface ISubscriberProvider
    {
        ISubscriber<TEventArgs> Get<TEventArgs>() where TEventArgs : EventArgs;
    }
}
