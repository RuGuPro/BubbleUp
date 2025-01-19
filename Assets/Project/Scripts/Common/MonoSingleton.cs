using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                {
                    Debug.LogWarning("创建实例");
                    instance = new GameObject().AddComponent<T>();
                   
                }
                if (instance == null)
                    Debug.LogWarning("没有找到实例");
            }
            return instance;
        }
    }
}