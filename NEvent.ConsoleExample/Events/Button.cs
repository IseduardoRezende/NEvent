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

        public void SendMessage(string message)
        {
            ArgumentNullException.ThrowIfNull(message);
            
            _ = _eventAggregator.PublishAsync(sender: this, new ButtonMessageArgs(message));
        }
    }
}
