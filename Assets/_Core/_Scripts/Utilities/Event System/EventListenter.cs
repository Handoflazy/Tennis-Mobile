using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utilities.Event_System;

namespace Event_System.Utilities.EventChannel
{
    public abstract class EventListenter<T1, T2> : MonoBehaviour
    {
        [SerializeField] private EventChannel<T1, T2> eventChannel;
        [SerializeField] private UnityEvent<T1, T2> unityEvent;
        
        public bool DebugLog = false;
        [EnableIf("DebugLog")]
        [SerializeField] private string debugLogMessage;
        private void Awake()
        {
            eventChannel.Register(this);
        }

        public void Raise(T1 value1, T2 value2)
        {
            if(DebugLog)
                Debug.Log($"Event raised: {value1}, {value2}");
            unityEvent?.Invoke(value1, value2);
        }

        private void OnDestroy()
        {
            eventChannel.DeRegister(this);
        }
    }
}