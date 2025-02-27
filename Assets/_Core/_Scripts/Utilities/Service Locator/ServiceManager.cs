using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace UnityServiceLocator
{
    public class ServiceManager
    {
        readonly Dictionary<Type, object> services = new();
        public IEnumerable<object> RegisteredServices => services.Values;
        
        public bool TryGet<T>(out T service) where T: class {
            Type type = typeof(T);
            if(services.TryGetValue(type, out object value)) {
                service = value as T;
                return true;
            }
            service = null;
            return false;
        }
        
        public T Get<T>() where T: class{
            Type type = typeof(T);
            if(services.TryGetValue(type, out object obj)) {
                return obj as T;
            }
            throw new ArgumentException($"ServiceManager.Get: Service of type {type.FullName} is not registered");
        }
        public ServiceManager Register<T>(T service)
        {
           Type type = typeof(T);
           if(!services.TryAdd(type, service))
               throw new InvalidOperationException($"Service of type {type.FullName} is already registered");
           return this;
        }

        public ServiceManager Register(Type type, object service) {
            if(!type.IsInstanceOfType(service)) {
                throw new AggregateException("Service is not of the correct type");
            }

            if(!services.TryAdd(type, service)) {
                throw new InvalidOperationException($"Service of type {type.FullName} is already registered");
            }
            return this;
        }
        public void Unregister<T>() {
            Type type = typeof(T);
            if(!services.Remove(type))
                throw new InvalidOperationException($"Service of type {type.FullName} is not registered");
        }
        public void Unregister(Type type) {
            if(!services.Remove(type))
                throw new InvalidOperationException($"Service of type {type.FullName} is not registered");
        }
        public void Clear() {
            services.Clear();
        }
        
    }
}