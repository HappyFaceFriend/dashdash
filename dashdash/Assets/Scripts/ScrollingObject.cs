using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScrollingObject : MonoBehaviour
{
    public float scrollAlpha = 1f;
    public float height;
    protected void Scroll()
    {
        transform.position += (new Vector3(0f, -GameManager.Instance.scrollSpeed * scrollAlpha, 0f) * Time.deltaTime);
        if(transform.position.y <= -Defs.GameHeight/2 -height/2)
        {
            OutOfScreen();
        }
    }

    protected abstract void OutOfScreen();
}
