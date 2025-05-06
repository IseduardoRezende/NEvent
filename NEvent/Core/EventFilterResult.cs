namespace NEvent.Core
{
    public enum EventFilterResult
    {
        Proceed,
        Skip,
        SkipHandlers,
        CancelCompletely
    }
}
