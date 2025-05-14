using NEvent.Interfaces;

namespace NEvent.ConsoleExample.Events
{
    public class Button(IEventAggregator eventAggregator)
    {
        private readonly IEventAggregator _eventAggregator = eventAggregator;

        public void ClickNumberIncrement()
        {
            _ = _eventAggregator.PublishAsync(sender: this, new ButtonClickArgs());
        }

        public async Task SendMessageAsync(string message)
        {
            ArgumentNullException.ThrowIfNull(message);

            await _eventAggregator.PublishAsync(sender: this, new ButtonMessageArgs(message));
        }
    }
}
