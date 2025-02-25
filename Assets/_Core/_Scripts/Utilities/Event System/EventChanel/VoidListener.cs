using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.EventChannel
{
    public class VoidListener : MonoBehaviour
    {
        [SerializeField] private VoidEventChannel eventChannel;
        [SerializeField] private UnityEvent unityEvent;

        private void Awake()
        {
            eventChannel.Register(this);
        }
        public void Raise()
        {
            unityEvent?.Invoke();
        }
        
        private void OnDestroy()
        {
            eventChannel.DeRegister(this);
        }
    }
}