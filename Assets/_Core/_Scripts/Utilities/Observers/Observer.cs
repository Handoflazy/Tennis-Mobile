
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.Events;
#else
#endif
[Serializable]
public class Observer<T>
{
    [SerializeField] private T value;
    [SerializeField] private UnityEvent<T> onValueChanged;

    
    public T Value
    {
        get => value;
        set => Set(value);
    }

    public static implicit operator T(Observer<T> observer) => observer.value;
    public Observer(T value, UnityAction<T> callback = null)
    {
        this.value = value;
        onValueChanged = new UnityEvent<T>();
        if(callback!=null) onValueChanged.AddListener(callback);
    }

    public void Set(T value)
    {
        if (Equals(this.value, value))
            return;
        this.value = value;
        Invoke();
    }

    public void Invoke()
    {
        onValueChanged.Invoke(value);
    }

    public void AddListener(UnityAction<T> callback)
    {
        if (callback == null) return;
        if (onValueChanged == null) onValueChanged = new UnityEvent<T>();
        
#if UNITY_EDITOR
        UnityEventTools.AddPersistentListener(onValueChanged, callback);
#else
        onValueChanged.AddListener(callback);
#endif
        
    }
    public void RemoveListener(UnityAction<T> callback)
    {
        if (callback == null) return;
        if (onValueChanged == null) onValueChanged = new UnityEvent<T>();
        
#if UNITY_EDITOR
        UnityEventTools.RemovePersistentListener(onValueChanged, callback);
#else
        onValueChanged.RemoveListener(callback);
#endif
    }

    public void RemoveAllListener()
    {
        if (onValueChanged == null) return;
#if UNITY_EDITOR
        FieldInfo fieldInfo =
            typeof(UnityEventBase).GetField("m_PersistentCalls", BindingFlags.Instance | BindingFlags.Public);
        object value = fieldInfo.GetValue(onValueChanged);
        value.GetType().GetMethod("Clear").Invoke(value, null);
#else
        onValueChanged.RemoveAllListeners();
#endif
    }

    public void Dispose()
    {
        RemoveAllListener();
        onValueChanged = null;
        value = default;
    }
}

