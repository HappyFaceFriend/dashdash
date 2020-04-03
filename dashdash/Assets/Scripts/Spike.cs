using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : ScrollingObject
{
    public float xPos;
    Animator animator;
    Vector3 originalScale;


    void Awake()
    {
        originalScale = transform.localScale;
        animator = GetComponent<Animator>();
    }
    public void Initialize(int dir, bool second = false)
    {
        transform.eulerAngles = new Vector3(0,0,90);
        transform.localScale = originalScale;
        xPos = Mathf.Abs(xPos);
        if(dir < 0)
        {
            xPos *= -1;
            transform.eulerAngles = new Vector3(0,0,-90);
        }
        transform.position = new Vector3(xPos, Defs.GameHeight/2f + height/2f, 0f);
        if(second)
            transform.position += new Vector3(0f, height, 0f);
        gameObject.SetActive(true);
    }
    protected override void OutOfScreen()
    {
        animator.SetTrigger("GameStart");
        PoolManager.Instance.ReturnObject(Defs.Spike, gameObject);
    }
    void Update()
    {
        Scroll();
    }
    public void GameOver()
    {  
        animator.SetTrigger("GameOver");
    }
    public void ColWithStomp()
    {
        int dir=1;
        if(xPos < 0)
            dir = -1;
        ScrollingParticle die = PoolManager.Instance.GetObject<ScrollingParticle>(Defs.SpikeDie);
        die.Initialize(Defs.SpikeDie, transform.position + new Vector3(60*dir,30f,300f), dir*90f);
        OutOfScreen();
    }
}
