using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{

    public float scrollSpeed;
    public float targetScrollSpeed;

    public ScoreText scoreText;
    public int score = 0;

    public Animator canvasAnimator;

     void Awake()
    {
        GameManager.Instance.targetScrollSpeed = targetScrollSpeed;
        GameManager.Instance.scoreText = scoreText;
        GameManager.Instance.canvasAnimator = canvasAnimator;
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(Defs.GameOverScene);
    }
}
