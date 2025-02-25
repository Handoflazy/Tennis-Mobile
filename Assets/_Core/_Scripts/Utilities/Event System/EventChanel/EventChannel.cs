using System;
using System.Collections.Generic;
using Platformer;
using UnityEngine;

namespace Utilities.EventChannel
{
    public abstract class EventChannel<T> : ScriptableObject
    {
        private readonly HashSet<EventListenter<T>> observers = new();

        public void Invoke(T value)
        {
            foreach (var observer in observers)
            {
                observer.Raise(value);
            }
        }

        public void Register(EventListenter<T> observer) => observers.Add(observer);
        public void DeRegister(EventListenter<T> observer) => observers.Remove(observer);
    }
}
