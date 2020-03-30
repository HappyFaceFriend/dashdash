using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    Queue<GameObject> pool;
    public GameObject prefab;
    public int count;
    List<GameObject> activeObjects;

    void Awake()
    {
        pool = new Queue<GameObject>();
        activeObjects = new List<GameObject>();
        for(int i=0; i<count; i++)
            CreateNewObject();
    }
    public void ChangePrefab(GameObject prefab)
    {
        this.prefab = prefab;
        while(pool.Count > 0)
            Destroy(pool.Dequeue());
        for(int i=0; i<count; i++)
            CreateNewObject();
    }
    void CreateNewObject()
    {
        GameObject ob = Instantiate(prefab,transform);
        ob.SetActive(false);
        pool.Enqueue(ob);
    }

    public GameObject GetObject(bool setActive = true)
    {
        if(pool.Count == 0)
            CreateNewObject();
        GameObject ob = pool.Dequeue();
        ob.SetActive(setActive);
        activeObjects.Add(ob);
        return ob;
    }
    public void ReturnObject(GameObject ob)
    {
        ob.SetActive(false);
        pool.Enqueue(ob);
        activeObjects.Remove(ob);
    }
    public List<T> GetActiveObjects<T>()
    {
        List<T> list = new List<T>();
        foreach(GameObject g in activeObjects)
            list.Add(g.GetComponent<T>());
        return list;
    }
}
