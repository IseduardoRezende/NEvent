using NEvent.Core;
using NEvent.Interfaces;

namespace NEvent.ConsoleExample.Events
{
    [EventOrder(2)]
    public class ButtonMessageHandler : IEventHandler<ButtonMessageArgs>
    {       
        public async Task HandleAsync(object _, ButtonMessageArgs args, CancellationToken cancellationToken = default)
        {            
            Console.WriteLine(args.Message);

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Red;
            
            Console.WriteLine("ButtonMessageHandler Executed!");
            await Task.Delay(300, cancellationToken);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;        
        }
    }
}
