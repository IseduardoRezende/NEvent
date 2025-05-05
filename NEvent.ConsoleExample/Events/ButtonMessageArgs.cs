namespace NEvent.ConsoleExample.Events
{
    public class ButtonMessageArgs(string message) : EventArgs
    {
        public string Message => message;
    }
}
