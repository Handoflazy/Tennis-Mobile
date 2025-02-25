using System;
using System.Collections.Generic;

namespace Utilities.Event_System.EventBus
{


    public static class EventBus<T> where T : IEvent
    {
        private static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();
        public static void Register(EventBinding<T> binding) => bindings.Add(binding);
        public static void Deregister(EventBinding<T> binding) => bindings.Remove(binding);
        
        public static void Raise(T @event)
        {
            var bindingsCopy = new List<IEventBinding<T>>(bindings);
            foreach (var binding in bindingsCopy)
            {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }

        static void Clear()
        {
            bindings.Clear();
        }
    }
}