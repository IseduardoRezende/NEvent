using NEvent.Core;
using NEvent.Interfaces;

namespace NEvent.ConsoleExample.Events
{
    [EventOrder(1)]
    public class ButtonMessageFilterTwo : IEventFilter<ButtonMessageArgs>
    {
        public Task OnAfterPublishAsync(object sender, ButtonMessageArgs args, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<EventFilterResult> OnBeforePublishAsync(object sender, ButtonMessageArgs args, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(EventFilterResult.SkipHandlers);
        }
    }
}
