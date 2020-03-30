using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    int score;
    public Text frontText;
    public Text backText;
    float originalY;
    RectTransform rectTransform;

    public bool shake = true;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalY = rectTransform.position.y;
    }
    public void SetScore(int score)
    {
        if(score == this.score)
            return;
        frontText.text = ""+ score;
        backText.text =  ""+ score;
        this.score = score;
        if(shake)
        rectTransform.position -= new Vector3(0f,20f,0f);
    }
    void Update()
    {
        if(shake && rectTransform.position.y <= originalY)
        {
            rectTransform.position += new Vector3(0,20f,0f) * Time.deltaTime * 5f;
        }
    }
}
