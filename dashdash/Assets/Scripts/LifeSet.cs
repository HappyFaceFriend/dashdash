using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeSet : MonoBehaviour
{
    public Image [] images;
    public Sprite lifeFull;
    public Sprite lifeEmpty;
    int life;

    void Awake()
    {
        life = 3;
        StartCoroutine(AnimateCreation());
    }

    public void Revive()
    {
        images[0].sprite = lifeFull;
        StartCoroutine(SquishHeart(0));
    }
    public void GetDamage()
    {
        if(life == 0)
            return;
        images[life-1].sprite = lifeEmpty;
        StartCoroutine(SquishHeart(life-1));
        StartCoroutine(Funcs.Shake(images[life-1].GetComponent<RectTransform>(),15,0.8f));
        life --;
    }

    IEnumerator SquishHeart(int index)
    {
        RectTransform rt = images[index].GetComponent<RectTransform>();
        rt.localScale = new Vector3(0.85f, 0.85f, 1f);
        yield return new WaitForSeconds(0.16f);
        rt.localScale = new Vector3(1f, 1f, 1f);
    }
    IEnumerator AnimateCreation()
    {
        yield return new WaitForSeconds(0.7f);
        for(int i=0; i<3; i++)
        {
            images[i].sprite = lifeFull;
            StartCoroutine(SquishHeart(i));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
