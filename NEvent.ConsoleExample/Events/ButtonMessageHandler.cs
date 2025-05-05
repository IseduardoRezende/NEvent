using NEvent.Interfaces;

namespace NEvent.ConsoleExample.Events
{
    public class ButtonMessageHandler : IEventHandler<ButtonMessageArgs>
    {
        public Task HandleAsync(object sender, ButtonMessageArgs args, CancellationToken _)
        {            
            Console.WriteLine(args.Message);

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Red;
            
            Console.WriteLine("Te amo Liv | I Love you Liv");
            Thread.Sleep(500);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        
            return Task.CompletedTask;
        }
    }
}
