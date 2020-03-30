using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundOffset : MonoBehaviour
{
    public Transform [] alpha10bg;
    public Transform [] alpha08bg;
    public Transform [] alpha06bg;
    

    void Awake()
    {
        DataManager.Instance.scrolledDistance %= 23040;
        float bg10offset = DataManager.Instance.scrolledDistance % 1920;
        foreach(Transform t in alpha10bg)
            t.position -= new Vector3(0,DataManager.Instance.scrolledDistance % 1920,0);
        foreach(Transform t in alpha08bg)
            t.position -= new Vector3(0,DataManager.Instance.scrolledDistance*0.6f % 1920,0);
        foreach(Transform t in alpha06bg)
            t.position -= new Vector3(0,DataManager.Instance.scrolledDistance*0.8f % 1920,0);
    }
}
