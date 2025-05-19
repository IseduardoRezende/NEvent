using NEvent.Core;
using NEvent.Interfaces;

namespace NEvent.ConsoleExample.Events
{
    [EventOrder(1)]
    public class ButtonMessageHandlerTwo : IEventHandler<ButtonMessageArgs>
    {
        public Task HandleAsync(object sender, ButtonMessageArgs args, CancellationToken cancellationToken = default)
        {
            Console.WriteLine(args.Message.ToLower() + "- ButtonMessageHandlerTwo Executed!");
            return Task.CompletedTask;
        }
    }
}
