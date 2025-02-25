using System;
using Platformer;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.EventChannel
{
    public abstract class EventChannelSO<T> : ScriptableObject
    {
        private UnityAction<T> OnEventRaised;
        
        public void Invoke(T value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
        }
        public void Register(UnityAction<T> observer) => OnEventRaised += observer;
        public void DeRegister(UnityAction<T> observer) => OnEventRaised -= observer;
    }
    
    public abstract class EventChannelSO<T1,T2> : ScriptableObject
    {
        private UnityAction<T1, T2> OnEventRaised;
        
        public void Invoke(T1 value1, T2 value2)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value1, value2);
        }
        public void Register(UnityAction<T1,T2> observer) => OnEventRaised += observer;
        public void DeRegister(UnityAction<T1,T2> observer) => OnEventRaised -= observer;
    }
}