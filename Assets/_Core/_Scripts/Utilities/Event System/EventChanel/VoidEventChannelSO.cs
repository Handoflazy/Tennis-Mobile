using Platformer;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.EventChannel
{
    [CreateAssetMenu(menuName = "Events/Void ChannelSO")]
    public class VoidEventChannelSO : ScriptableObject
    {
        private UnityAction onEventRaised;
        
        public void Invoke()
        {
            if (onEventRaised != null)
                onEventRaised.Invoke();
        }
        public void Register(UnityAction observer) => onEventRaised += observer;
        public void DeRegister(UnityAction observer) => onEventRaised -= observer;
    }
}