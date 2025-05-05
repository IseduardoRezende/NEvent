namespace NEvent.Interfaces
{
    public interface IEventHandler<TEventArgs> where TEventArgs : EventArgs
    {
        Task HandleAsync(TEventArgs args, CancellationToken cancellationToken = default);
    }
}
