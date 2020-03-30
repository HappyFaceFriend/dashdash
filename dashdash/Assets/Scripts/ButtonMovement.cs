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
    int delta = 20;
    void Awake()
    {   
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.position;
        isDown = false;
    }
    public void PointerDown()
    {
        if(!button.interactable)
            return;
        isDown = true;
        rectTransform.position = new Vector3(rectTransform.position.x, originalPos.y-delta,0);
    }
    public void PointerUp()
    {
        isDown = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(!isDown && rectTransform.position.y < originalPos.y)
        {
            rectTransform.position += new Vector3(0,delta * Time.deltaTime * 5f, 0);
            if(rectTransform.position.y > originalPos.y)
                rectTransform.position = new Vector3(rectTransform.position.x, originalPos.y,0);
        }
    }
}