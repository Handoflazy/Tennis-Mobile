using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;

namespace Platformer._3DPlatformer._Scripts.Utilities.Observers
{
    [Serializable]
    public class ObservableArray<T> : IObservableArray<T>
    {
        private T[] items;

       // public static implicit operator T[](ObservableArray<T> arrays) => arrays.items;

        public T[] Items => items;
        

        public event Action<T[]> AnyValueChanged = delegate { };
        public int Count => items.Count(i => i != null);

        public T this[int index] => items[index];
        
        public ObservableArray(int size, IList<T> initialList = null)
        {
            items = new T[size];
            if (initialList != null)
            {
                initialList.Take(size).ToArray().CopyTo(items, 0);
                Invoke();
            }
        }
        public ObservableArray(T[] initialArray)
        {
            items = initialArray;
            Invoke();
        }
        
        public bool IsIndexNotNull(int index) => items[index]!=null;
        public void Invoke() => AnyValueChanged.Invoke(items);

        public void Swap(int index1, int index2)
        {
            (items[index1], items[index2]) = (items[index2], items[index1]);
            Invoke();
        }

        public void Clear()
        {
            items = new T[items.Length];
            Invoke();
        }

        public bool TryAdd(T item)
        {
            for (var i = 0; i < items.Length; i++) {
                if (TryAddAt(i, item)) return true;
            }
            return false;
        }

        public bool TryAddAt(int index, T item) {
            if (index < 0 || index >= items.Length) return false;
        
            if (items[index] != null) return false;

            items[index] = item;
            Invoke();
            return true;
        }

        public bool TryRemove(T item)
        {
            for (var i = 0; i < items.Length; i++) {
                if (EqualityComparer<T>.Default.Equals(items[i], item) && TryRemoveAt(i)) return true;
            }
            return false;
        }
        public bool TryRemoveAt(int index) {
            if (index < 0 || index >= items.Length) return false;
        
            if (items[index] == null) return false;

            items[index] = default;
            Invoke();
            return true;
        }

      }
}