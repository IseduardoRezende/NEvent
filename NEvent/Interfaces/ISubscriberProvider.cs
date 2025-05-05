namespace NEvent.Interfaces
{
    public interface ISubscriberProvider
    {
        public ISubscriber<TEventArgs> GetSubscriber<TEventArgs>() where TEventArgs : EventArgs;
    }
}
