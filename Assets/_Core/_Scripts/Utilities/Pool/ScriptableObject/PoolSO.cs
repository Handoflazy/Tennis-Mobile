using System.Collections.Generic;
using Platformer.Factory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer.Pool
{
    
    /// <summary>
    /// A generic pool that generates members of type T on-demand via a factory.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements to pool.</typeparam>
    public abstract class PoolSO<T>: ScriptableObject, IPool<T> 
    {
        protected readonly Stack<T> available = new Stack<T>();
        public abstract IFactory<T> Factory { get; set; }
        protected bool HasBeenPrewarmed { get; set; }
        public virtual void OnDisable()
        {
            available.Clear();
            HasBeenPrewarmed = false;
        }

        protected virtual T Create() => Factory.Create();


        public void Prewarm(int num)
        {
            if (HasBeenPrewarmed)
            {
                Debug.LogWarning($"Pool {name} has already been prewarmed.");
                return;
            }
            for (int i = 0; i < num; i++)
            {
                available.Push(Create());
            }
            HasBeenPrewarmed = true;
        }

        public virtual T Request()
        {
            return available.Count > 0 ? available.Pop() : Create();
        }

        public virtual IEnumerable<T> Request(int num = 1)
        {
            List<T> members = new List<T>(num);
            for (int i = 0; i < num; i++)
            {
                members.Add(Request());
            }
            return members;
        }

        public virtual void Return(T member)
        {
                available.Push(member);
        }

        public virtual void Return(IEnumerable<T> members)
        {
            foreach (T member in members)
            {
                Return(member);
            }
        }
    }
}