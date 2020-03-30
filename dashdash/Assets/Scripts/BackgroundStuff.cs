using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStuff : ScrollingObject
{
    void Awake()
    {
        
    }
    void Update()
    {
        Scroll();
    }
    protected override void OutOfScreen()
    {
        transform.position += new Vector3(0, Defs.GameHeight + height, 0f);
    }
}
