using NEvent.Core;

namespace NEvent.Interfaces
{
    public interface IEventFilter<TEventArgs> where TEventArgs : EventArgs
    {        
        Task<EventFilterResult> OnBeforePublishAsync(object sender, TEventArgs args, CancellationToken cancellationToken = default);

        Task OnAfterPublishAsync(object sender, TEventArgs args, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }           
    }
}
