
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;


//Destroy old instead new
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<T>();
                if (instance == null)
                {
                    var go = new GameObject(typeof(T).Name + "Auto Generated");
                    instance = go.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    public static bool HasInstance => Instance != null;
    public static T TryGetInstance() => HasInstance ? Instance : null;
    protected virtual void Awake()
    {
        InitializingInstance();
    }

    protected virtual void InitializingInstance()
    {
        if (!Application.isPlaying) return;
        
        instance = this as T;
    }

    protected virtual void OnApplicationQuit() {
        Debug.Log("OnApplicationQuit");
        if (HasInstance)
            instance = null;
    }
}

//Basic Singleton

public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (HasInstance)
        {
            Destroy(gameObject);
            return;
        };
        base.Awake();
    }
}

// Persistent version

public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
     public bool AutoUnParentOnAwake = false;
    protected override void Awake()
    {
        
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    protected override void InitializingInstance()
    {
       
        if (!Application.isPlaying) return;
        if(AutoUnParentOnAwake)
            transform.SetParent(null);
        if (!instance||instance != this)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    protected override void OnApplicationQuit() {
        base.OnApplicationQuit();
        Debug.Log("Persisten end");
    }
}

public abstract class RegulatorSingleton<T>:MonoBehaviour where T : Component
{
    private static T instance;
    public float InitilizationTime { get; private set; }
    public static T Instance
    {
        get
        {
            if (instance is null)
            {
                instance =FindFirstObjectByType<T>();
                if (instance is null)
                {
                    var go = new GameObject(typeof(T).Name + "Auto Generated");
                    instance = go.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    public static bool HasInstance => Instance != null;
    protected virtual void Awake()
    {
        InitializingInstance();
    }

    protected virtual void InitializingInstance()
    {
        if (!Application.isPlaying) return;
        InitilizationTime = Time.time;
        DontDestroyOnLoad(this);
        
        T[] oldInstances = FindObjectsByType<T>(FindObjectsSortMode.None);
        foreach (T old in oldInstances)
        {
            if(old.GetComponent<RegulatorSingleton<T>>().InitilizationTime < InitilizationTime)
                Destroy(old.gameObject);
        }
        instance??=this as T;
    }

    protected virtual void OnApplicationQuit()
    {
        if (HasInstance)
            Destroy(gameObject);
    }
}


