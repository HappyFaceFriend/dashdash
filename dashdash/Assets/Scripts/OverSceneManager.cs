using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement   ;

public class OverSceneManager : MonoBehaviour
{
    public int score;
    public int highScore;
    int recentHighScore;

    public ScoreText scoreText;
    public ScoreText highScoreText;

    public Animator canvasAnimator;
    public Animator bestAnimator;

    bool gotoGame;
    // Start is called before the first frame update
    void Start()
    {
        score = DataManager.Instance.recentScore;
        highScore = DataManager.Instance.highScore;
        recentHighScore = DataManager.Instance.recentHighScore;
        
        if(score < highScore)
            highScoreText.SetScore(highScore);
        else
            highScoreText.SetScore(recentHighScore);
        StartCoroutine(CountScore());
    }
    IEnumerator CountScore()
    {
        int curScore = 0;
        int r = 3;
        if(score >= 60 * r)
            r = score/60;       
        while(curScore < score)
        {
            curScore += Random.Range(r-1,r+1);
            if(curScore > score)
                curScore = score;
            scoreText.SetScore(curScore);
            yield return null;
        }
        if(highScore == score)
            canvasAnimator.SetTrigger("Best");
    }
    public void BestAnimDone()
    {
        bestAnimator.SetTrigger("CanvasDone");
        if(highScore == score)
            StartCoroutine(CountHighScore());
    }
    IEnumerator CountHighScore()
    {
        int curScore = recentHighScore;
        int r = 3;
        if(highScore - recentHighScore >= 40 * r)
            r = (highScore - recentHighScore)/60;       
        while(curScore < score)
        {
            curScore += Random.Range(r-1,r+1);
            if(curScore > highScore)
                curScore = highScore;
            highScoreText.SetScore(curScore);
            yield return null;
        }
    }
    public void Restart()
    {
        canvasAnimator.SetTrigger("Close");
        gotoGame = true;
    }
    public void Select()
    {
        canvasAnimator.SetTrigger("Close");
        gotoGame = false;
    }
    public void OpenGameScene()
    {
        if(gotoGame)
            SceneManager.LoadScene(Defs.GameScene);
        else
            SceneManager.LoadScene(Defs.MainScene);
    }
}
