using NEvent.Core;
using NEvent.Interfaces;

namespace NEvent.ConsoleExample.Events
{
    public class ButtonMessageFilter : IEventFilter<ButtonMessageArgs>
    {
        public Task<EventFilterResult> OnBeforePublishAsync(object sender, ButtonMessageArgs args, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(string.IsNullOrWhiteSpace(args?.Message) || args.Message.Contains("Hi")
                 ? EventFilterResult.Skip
                 : EventFilterResult.Proceed); //or default
        }       
    }
}
