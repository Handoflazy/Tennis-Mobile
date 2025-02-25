using System;
using System.Collections;
using System.Collections.Generic;

public class ObservableList<T>: IList<T>, IObservableList<T>
{
    private readonly IList<T> list;
    public event Action<IList<T>> AnyValueChanged;
    
    public ObservableList(IList<T> initialList = null) {
        list = initialList ?? new List<T>();
    }

    void Invoke() => AnyValueChanged?.Invoke(list);
    public T this[int index]
    {
        get => list[index];
        set {
            list[index] = value;
            Invoke();
        }
    }
    public int Count => list.Count;
    public bool IsReadOnly => list.IsReadOnly;
    
    public void RemoveAt(int index)
    {
        T item = list[index];
        list.RemoveAt(index);
        Invoke();
    }

    IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

    public void Add(T item)
    {
        list.Add(item);
        Invoke();
    }

    public void Insert(int index, T item)
    {
        list.Insert(index, item);
        Invoke();
    }

    public void Clear()
    {
        list.Clear();
        Invoke();
    }

    public bool Contains(T item) => list.Contains(item);
    

    public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

    public int IndexOf(T item)
        => list.IndexOf(item);
    
    public bool Remove(T item)
    {
        var result = list.Remove(item);
        if (result) {
            Invoke();
        }

        return result;
    }

    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
}