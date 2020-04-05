using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdSet : MonoBehaviour
{
    public Animator canvasAnimator;
    public bool exit = true;
    public void AnimDone()
    {
        if(exit)
            canvasAnimator.SetTrigger("EndScene");
        Destroy(gameObject);
    }
}
