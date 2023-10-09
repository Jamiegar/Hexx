using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static instance does not destroy any new instances, it overrides the current 
/// instance. This can be used for resetting the state of an object.
/// </summary>

public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        instance = this as T;
    }

    protected virtual void OnApplicationQuit()
    {
        instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// This uses the static instance and turns it into a basic singleton. 
/// The basic singleton will destroy any new instances created.
/// </summary>

public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (instance == null)
        {
            base.Awake();
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

    }
}

/// <summary>
/// A Persistent singleton uses DontDestroyOnLoad() so will dont be destroyed when a scene loads.
/// </summary>
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}

