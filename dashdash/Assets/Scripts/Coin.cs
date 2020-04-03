using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : ScrollingObject
{
    SpriteRenderer sr;
    public bool isDead = false;
    Animator animator;
    Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
        height = transform.localScale.y * 100f;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    public void Initialize(int x)
    {
        transform.localScale = originalScale;
        transform.position = new Vector3(x, Defs.GameHeight/2f + height/2f, 0f);
        scrollAlpha = 1f;
        sr.color = new Color(1f,1f,1f,1f);
        isDead = false;
        gameObject.SetActive(true);
    }
    protected override void OutOfScreen()
    {
        animator.SetTrigger("GameStart");
        PoolManager.Instance.ReturnObject(Defs.Coin, gameObject);
    }
    void Update()
    {
        Scroll();
    }
    public void ColWithPlayer()
    {
        scrollAlpha = 0f;
        isDead = true;
        ScrollingParticle die = PoolManager.Instance.GetObject<ScrollingParticle>(Defs.CoinDie);
        die.Initialize(Defs.CoinDie, transform.position + new Vector3(0f,0f,500f), 0);
        die.sound.pitch = Random.Range(0.99f,1.01f);
        StartCoroutine(Disappear());
    }
    public void GameOver()
    {
        animator.SetTrigger("GameOver");
    }
    IEnumerator Disappear()
    {
        float duration = 0.5f;
        float eTime = 0f;
        while(eTime <= duration)
        {
            eTime += Time.deltaTime;
            transform.position += new Vector3(0f, 120f, 0f) * Time.deltaTime;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f - eTime/duration);
            yield return null;
        }
        OutOfScreen();
    }
}
