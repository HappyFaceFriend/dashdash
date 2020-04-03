using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingParticle : ScrollingObject
{
    public string poolName;
    public AudioSource sound;
    public void Initialize(string poolName, Vector3 position, float rotation, float scale)
    {
        this.poolName = poolName;
        transform.position = position;
        transform.Rotate(new Vector3(0,0,1), rotation);
        transform.localScale = new Vector3(scale,scale,1f);
        gameObject.SetActive(true);
        if(sound != null)
            sound.Play();
    }
    public void Initialize(string poolName, Vector3 position, float rotation)
    {
        this.poolName = poolName;
        transform.position = position;
        transform.Rotate(new Vector3(0,0,1), rotation);
        transform.localScale = new Vector3(1f,1f,1f);
        gameObject.SetActive(true);
        if(sound != null)
            sound.Play();
    }
    protected override void OutOfScreen()
    {
        transform.rotation = Quaternion.identity;
        PoolManager.Instance.ReturnObject(poolName, gameObject);
    }
    void OnParticleSystemStopped()
    {
        OutOfScreen();
    }
    void Update()
    {
        Scroll();
    }
}
