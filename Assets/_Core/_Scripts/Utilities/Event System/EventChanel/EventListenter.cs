using UnityEngine;
using UnityEngine.Events;

namespace Utilities.EventChannel
{
    public abstract class EventListenter<T> : MonoBehaviour
    {
        [SerializeField] private EventChannel<T> eventChannel;
        [SerializeField] private UnityEvent<T> unityEvent;
        
        public bool DebugLog = false;
        [SerializeField] private string debugLogMessage;
        private void Awake()
        {
             eventChannel.Register(this);
        }

        public void Raise(T value)
        {
            if(DebugLog)
                Debug.Log($"Event raised: {value}");
            unityEvent?.Invoke(value);
        }

        private void OnDestroy()
        {
            eventChannel.DeRegister(this);
        }
    }
}