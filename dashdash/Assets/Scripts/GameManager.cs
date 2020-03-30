using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float scrollSpeed;
    public float targetScrollSpeed;

    public ScoreText scoreText;
    public int score = 0;

    public Animator canvasAnimator;
    public float readyDuration = 2f;

    Coroutine gameStart;
    void Update()
    {
        DataManager.Instance.scrolledDistance += Time.deltaTime * scrollSpeed;
    }

    public void StartGame()
    {
        score = 0;
        gameStart = StartCoroutine(GamePrep());
    }

    public void EndGame()
    {
        if(gameStart != null)
            StopCoroutine(gameStart);
        StartCoroutine(GameEnd());  
        DataManager.Instance.recentScore = score;
        if(DataManager.Instance.highScore < score)
            DataManager.Instance.SetHighScore(score);
    }
    IEnumerator GamePrep()
    {
        float eTime = 0f;
        while(eTime <= readyDuration)
        {
            eTime += Time.deltaTime;
            scrollSpeed = Mathf.Lerp(0f, targetScrollSpeed, eTime/readyDuration);
            yield return null;
        }
        scrollSpeed = targetScrollSpeed;
        yield return null;
    }
    IEnumerator GameEnd()
    {
        
        float eTime = 0f;
        float duration = 0.5f;
        float fromSpeed = scrollSpeed;
        while(eTime <= duration)
        {
            eTime += Time.deltaTime;
            scrollSpeed = Mathf.Lerp(fromSpeed, 0, eTime/duration);
            yield return null;
        }
        scrollSpeed = 0;
        yield return new WaitForSeconds(0.1f);
        DestroyObjects();
        yield return new WaitForSeconds(0.3f);
        canvasAnimator.SetTrigger("EndScene");
    }
    void DestroyObjects()
    {
        List<Spike> spikes = PoolManager.Instance.GetActiveObjects<Spike>(Defs.Spike);
        List<Coin> coins = PoolManager.Instance.GetActiveObjects<Coin>(Defs.Coin);
        foreach (Spike spike in spikes)
            spike.GameOver();
        foreach (Coin coin in coins)
            coin.GameOver();
    }
    public void AddScore(int add)
    {
        score += add;
        scoreText.SetScore(score);
    }
    public void StompSpike(Vector3 pos)
    {
        AddScore(5);
        CreateEffect(pos + new Vector3(Random.Range(0f,1f) * 60,Random.Range(0f,1f) * 80,0 ),"5");
    }
    public void EatCoin(Vector3 pos)
    {
        AddScore(1);
        CreateEffect(pos + new Vector3(Random.Range(-1f,1f) * 20,Random.Range(-1f,1f) * 20,0 ),"1");
    }
    void CreateEffect(Vector3 pos, string eff)
    {
        ScoreEffect effect = PoolManager.Instance.GetObject<ScoreEffect>(eff);
        effect.Initialize(pos + new Vector3(Random.Range(-50,50), 130, 0));
    }
}
