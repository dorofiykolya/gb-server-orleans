namespace Common.Composite.Events
{
    public delegate void EventListenerDelegate<in T>(T evt) where T : Event;
}
