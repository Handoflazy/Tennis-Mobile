using Obvious.Soap;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Soap/EventListeners/EventListener"+nameof(Player))]
public class EventListenerPlayer : EventListenerGeneric<Player>
{
    [SerializeField] private EventResponse[] _eventResponses = null;
    protected override EventResponse<Player>[] EventResponses => _eventResponses;

    [System.Serializable]
    public class EventResponse : EventResponse<Player>
    {
        [SerializeField] private ScriptableEventPlayer _scriptableEvent = null;
        public override ScriptableEvent<Player> ScriptableEvent => _scriptableEvent;

        [SerializeField] private PlayerUnityEvent _response = null;
        public override UnityEvent<Player> Response => _response;
    }

    [System.Serializable]
    public class PlayerUnityEvent : UnityEvent<Player>
    {
        
    }
}
