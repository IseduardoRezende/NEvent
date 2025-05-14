using NEvent.Interfaces;

namespace NEvent.ConsoleExample.Events
{
    public class ButtonMessageHandler : IEventHandler<ButtonMessageArgs>
    {       
        public async Task HandleAsync(object _, ButtonMessageArgs args, CancellationToken cancellationToken = default)
        {            
            Console.WriteLine(args.Message);

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Red;
            
            Console.WriteLine("I Love you Liv");
            await Task.Delay(300, cancellationToken);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;        
        }
    }
}
