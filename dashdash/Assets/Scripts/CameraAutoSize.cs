using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAutoSize : MonoBehaviour
{
    Camera thisCamera;
    public float defaultSize;
    public Vector2 defaultResolution;
    public enum Orientation { portrait, landscape}
    public Orientation orientation;
    void Awake()
    {
        thisCamera = GetComponent<Camera>();
        //  newsize = size / 지금화면비 * 나중화면비
        float res = defaultResolution.y/defaultResolution.x;
        if(orientation == Orientation.landscape)
            res = defaultResolution.x/defaultResolution.y;
        thisCamera.orthographicSize = defaultSize * res / ((float)Screen.width/Screen.height);
    }
}
