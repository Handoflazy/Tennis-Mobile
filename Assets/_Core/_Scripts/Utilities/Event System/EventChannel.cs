using System.Collections.Generic;
using Event_System.Utilities.EventChannel;
using Platformer;
using UnityEngine;

namespace Utilities.Event_System
{
    public abstract class EventChannel<T1, T2> : ScriptableObject
    {
        private readonly HashSet<EventListenter<T1, T2>> observers = new();

        public void Invoke(T1 value1, T2 value2)
        {
            foreach (var observer in observers)
            {
                observer.Raise(value1, value2);
            }
        }

        public void Register(EventListenter<T1, T2> observer) => observers.Add(observer);
        public void DeRegister(EventListenter<T1, T2> observer) => observers.Remove(observer);
    }
}
