namespace NEvent.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventOrderAttribute(int value) : Attribute
    {
        public int Value { get; } = value;
    }
}
