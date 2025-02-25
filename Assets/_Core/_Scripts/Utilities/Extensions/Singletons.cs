
using System.Runtime.CompilerServices;
using UnityEngine;


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

    protected virtual void OnApplicatinQuit()
    {
        if(HasInstance)
            Destroy(gameObject);
    }
}

//Basic Singleton

public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null)
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
    public bool AutoUnparentOnAwake = default;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    protected override void InitializingInstance()
    {
        if (!Application.isPlaying) return;
        if(AutoUnparentOnAwake)
            transform.SetParent(null);
        if (!HasInstance)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

public abstract class RegulatorSinglenton<T>:MonoBehaviour where T : Component
{
    protected static T instance;
    public float InitilizationTime { get; private set; }
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance =FindFirstObjectByType<T>();
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
            if(old.GetComponent<RegulatorSinglenton<T>>().InitilizationTime < InitilizationTime)
                Destroy(old.gameObject);
        }
        instance??=this as T;
    }

    protected virtual void OnApplicatinQuit()
    {
        if (HasInstance)
            Destroy(gameObject);
    }
}


