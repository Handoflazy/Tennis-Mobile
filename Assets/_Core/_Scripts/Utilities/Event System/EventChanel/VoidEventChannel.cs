using System;
using System.Collections.Generic;
using Platformer;
using UnityEngine;

namespace Utilities.EventChannel
{
    [CreateAssetMenu(menuName = "Events/Void Channel")]
    public class VoidEventChannel: ScriptableObject
    {
        private readonly HashSet<VoidListener> observers = new();
        public void Invoke()
        {
            foreach (var observer in observers)
            {
                observer.Raise();
            }
        }

        public void Register(VoidListener observer) => observers.Add(observer);
        public void DeRegister(VoidListener observer) => observers.Remove(observer);
    }
}