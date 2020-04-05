using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Funcs
{
    public static bool IsKorean()
    {
        SystemLanguage lang = Application.systemLanguage;
        if(lang == SystemLanguage.Korean)
            return true;
        else
            return false;
    }
    public static IEnumerator Shake(Transform t, float magnitude, float duration)
    {
        float eTime = 0f;
        float frameTime=0f;
        float m = magnitude;
        Vector3 originalPos = t.localPosition;
        while(eTime < duration)
        {
            eTime += Time.deltaTime;
            frameTime+= Time.deltaTime;
            t.localPosition = new Vector3(Random.Range(-1f,1f) * m, Random.Range(-1f,1f) * m,t.localPosition.z);
            
            m = magnitude * (1-eTime/duration) * Time.timeScale; 
            yield return null;
        }   
        t.localPosition = originalPos;
    }
    public static IEnumerator Shake(RectTransform t, float magnitude, float duration)
    {
        float eTime = 0f;
        float m = magnitude;
        Vector3 originalPos = t.localPosition;
        while(eTime < duration) 
        {
            eTime += Time.deltaTime;
            t.localPosition = originalPos + new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f),0) * m  * Time.timeScale;
            m = magnitude * (1-eTime/duration); 
            yield return null;
        }
        t.localPosition = originalPos;
    }
    public static Vector2 Vec3to2(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }
    public static Vector3 Vec2to3(Vector2 v, float z = 0)
    {
        return new Vector3(v.x, v.y, z);
    }
    public static Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public static bool IsPointerOverGameObject()
    {
        //check mouse
        if(EventSystem.current.IsPointerOverGameObject())
            return true;
        
        //check touch
        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began ){
            if(EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return true;
        }
        return false;
    }
}
