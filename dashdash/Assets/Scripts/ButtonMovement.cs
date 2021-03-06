﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMovement : MonoBehaviour
{
    Vector3 originalPos;
    RectTransform rectTransform;
    Button button;
    bool isDown;
    int delta = 10;
    void Awake()
    {   
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.localPosition;
        isDown = false;
    }
    public void PointerDown()
    {
        if(!button.interactable)
            return;
        isDown = true;
        rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, originalPos.y-delta,0);
        SoundManager.Instance.PlaySound(SoundManager.Effects.Touch);
    }
    public void PointerUp()
    {
        isDown = false;
        //SoundManager.Instance.PlaySound(SoundManager.Effects.Up);
    }
    // Update is called once per frame
    void Update()
    {
        if(!isDown && rectTransform.localPosition.y < originalPos.y)
        {
            rectTransform.localPosition += new Vector3(0,delta * Time.unscaledDeltaTime * 5f, 0);
            if(rectTransform.localPosition.y > originalPos.y)
                rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, originalPos.y,0);
        }
    }
}
