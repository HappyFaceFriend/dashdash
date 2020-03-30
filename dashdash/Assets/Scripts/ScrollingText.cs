using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingText : ScrollingObject
{
    protected override void OutOfScreen()
    {
        Destroy(gameObject);
    }
    void Update()
    {
        Scroll();
    }
}
