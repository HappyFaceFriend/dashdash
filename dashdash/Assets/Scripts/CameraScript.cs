using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Camera thisCamera;
    float originalSize;
    bool isShaking;
    void Awake()
    {
        thisCamera = GetComponent<Camera>();
    }
    void Start()
    {
        originalSize = thisCamera.orthographicSize;
    }
    public void ShakeJump()
    {
        thisCamera.orthographicSize *= 1.02f;
    }
    
    public void ShakeLand(bool isPowered)
    {
        if(!isShaking)
            StartCoroutine(Land(isPowered));
        else
            thisCamera.orthographicSize = originalSize;
    }
    public void ShakeSpike()
    {
        transform.position = new Vector3(0,0,-10);
        StartCoroutine(Spike());
    }
    IEnumerator Spike()
    {
        isShaking  = true;
        yield return StartCoroutine(Funcs.Shake(transform,35,0.3f));
        isShaking  = false;
    }
    IEnumerator Land(bool isPowered)
    {
        if(!isPowered)
        {    
            thisCamera.orthographicSize = originalSize;
            yield return StartCoroutine(Funcs.Shake(transform,18,0.15f));
        }
        else
        {
            thisCamera.orthographicSize = originalSize;
            yield return StartCoroutine(Funcs.Shake(transform,35,0.3f));
        }
    }
}
