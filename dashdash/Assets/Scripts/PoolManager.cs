using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    Dictionary<string, Pool> pools;

    void Start()
    {
        pools = new Dictionary<string, Pool>();
        foreach(Pool pool in GetComponentsInChildren<Pool>())
        {
            pools.Add(pool.gameObject.name, pool);
        }
    }
    public void SetPoolPrefab(string name, GameObject prefab)
    {
        pools[name].ChangePrefab(prefab);
    }
    public GameObject GetObject(string name)
    {
        return pools[name].GetObject();
    }
    public T GetObject<T>(string name, bool setActive = false)
    {
        return pools[name].GetObject(setActive).GetComponent<T>();
    }
    public List<T> GetActiveObjects<T>(string name)
    {
        return pools[name].GetActiveObjects<T>();
    }
    public void ReturnObject(string name, GameObject ob)
    {
        pools[name].ReturnObject(ob);
    }
}
