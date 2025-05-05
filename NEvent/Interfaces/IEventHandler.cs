namespace NEvent.Interfaces
{
    public interface IEventHandler<TEventArgs> where TEventArgs : EventArgs
    {
        Task HandleAsync(object sender, TEventArgs args, CancellationToken cancellationToken = default);
    }
}
