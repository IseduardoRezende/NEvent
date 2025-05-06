using NEvent.Interfaces;

namespace NEvent.ConsoleExample.Events
{
    public class ButtonClickHandler : IEventHandler<ButtonClickArgs>
    {        
        private int clickTimes = 0;

        public Task HandleAsync(object _, ButtonClickArgs __, CancellationToken ___)
        {
            clickTimes++;

            SetRandomConsoleForegroundColor();

            Console.WriteLine($"Button clicled: {clickTimes} times.");

            Console.ForegroundColor = ConsoleColor.Gray;

            return Task.CompletedTask;
        }

        private static void SetRandomConsoleForegroundColor()
        {
            ConsoleColor[] colors = Enum.GetValues<ConsoleColor>();
            Console.ForegroundColor = Random.Shared.GetItems(colors, colors.Length).First();
        }
    }
}
