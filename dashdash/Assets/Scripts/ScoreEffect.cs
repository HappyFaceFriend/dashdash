using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEffect : MonoBehaviour
{
    public string poolName;
    Color originalColor;
    SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
    }
    
    public void Initialize(Vector3 position)
    {
        transform.position = position;
        sprite.color = originalColor;
        gameObject.SetActive(true);
        StartCoroutine(Disappear());
    }
    
    IEnumerator Disappear()
    {
        float duration = 0.8f;
        float eTime = 0f;
        while(eTime <= duration)
        {
            eTime += Time.deltaTime;
            transform.position += new Vector3(0f, 60f, 0f) * Time.deltaTime;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f - eTime/duration);
            yield return null;
        }
        
        PoolManager.Instance.ReturnObject(poolName, gameObject);
    }
}
