using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class DataManager : Singleton<DataManager>
{
    public int recentScore;
    public int highScore;
    public int recentHighScore;

    public float scrolledDistance; 
    public int character = 1;

    public int howToPlay;

    public bool muteSound;
    public bool muteMusic;

    void Awake()
    {
        base.Awake();
        howToPlay = PlayerPrefs.GetInt("howToPlay", 0);
        highScore = PlayerPrefs.GetInt("highScore", 0);
        recentHighScore = PlayerPrefs.GetInt("recentHighScore", 0);
        int d;
        d = PlayerPrefs.GetInt("muteSound", 0);
        muteSound = d==0?false:true;
        d = PlayerPrefs.GetInt("muteMusic", 0);
        muteMusic = d==0?false:true;
    }
    public void SetSound(bool mute)
    {
        muteSound = mute;
        PlayerPrefs.SetInt("muteSound", mute?1:0);
    }
    public void SetMusic(bool mute)
    {
        muteMusic = mute;
        PlayerPrefs.SetInt("muteMusic", mute?1:0);
    }
    public void SaveHowToPlay(int i)
    {
        //0 : 처음봄   1 : 끔   2 : 계속보게하기
        PlayerPrefs.SetInt("howToPlay", i);
        howToPlay = i;
    }
    public void SetHighScore(int highScore)
    {
        recentHighScore = this.highScore;
        this.highScore = highScore;
        PlayerPrefs.SetInt("highScore", highScore);
        PlayerPrefs.SetInt("recentHighScore", recentHighScore);
    }
}
