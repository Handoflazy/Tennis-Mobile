namespace Utilities.Event_System.EventBus
{
    public interface IEvent{}
    
    public struct TestEvents : IEvent
    {
    }

    public struct PlayerEvents : IEvent
    {
        private int health;
    }
}