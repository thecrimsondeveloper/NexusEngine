using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{


    [ShowInInspector, FoldoutGroup("Singleton")] private static T instance = null;
    [SerializeField, FoldoutGroup("Singleton")] bool shouldPersistBetweenScenes = false;

    protected static T Instance
    {
        get
        {
            if (Application.isPlaying == false)
            {
                T temp = FindObjectOfType<T>();
                if (temp == null)
                {
                    Debug.LogError($"{typeof(T)} is not found in the scene.");
                }
                return temp;
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (shouldPersistBetweenScenes)
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        if (instance == null)
        {
            instance = (T)this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }
}
