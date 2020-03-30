using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance; 
    
    public static T Instance
    {
        get
        {
            if(instance==null)
            {
                GameObject ob = GameObject.Find(typeof(T).Name);
                if(ob == null)
                {
                    ob = new GameObject(typeof(T).Name);
                    ob.tag = typeof(T).Name;
                    instance = ob.AddComponent<T>();
                }
                else
                    instance = ob.GetComponent<T>();
            }
            return instance;
        }
    }

    public void Awake()
    {
        if(GameObject.FindGameObjectsWithTag(typeof(T).Name).Length > 1)
        {
            Destroy(gameObject);
        }
        else
            DontDestroyOnLoad(gameObject);
    }

}
