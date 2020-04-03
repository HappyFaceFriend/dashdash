using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{

    public float scrollSpeed;
    public float targetScrollSpeed;

    public ScoreText scoreText;
    public int score = 0;

    public Animator canvasAnimator;

    public Image music;
    public Image sound;

    public Sprite musicOn;
    public Sprite musicOff;
    public Sprite soundOn;
    public Sprite soundOff;

    public GameObject generator;
    public Animator howToPlay;

     void Awake()
    {
        GameManager.Instance.targetScrollSpeed = targetScrollSpeed;
        GameManager.Instance.scoreText = scoreText;
        GameManager.Instance.canvasAnimator = canvasAnimator;
    }
    void Start()
    {
        if(DataManager.Instance.muteSound)
            sound.sprite = soundOff;
        if(DataManager.Instance.muteMusic)
            music.sprite = musicOff;
        if(DataManager.Instance.howToPlay == 0)
        {
            howToPlay.gameObject.SetActive(true);
            generator.SetActive(false);
        }
    }
    public void RemoveHowToPlay()
    {
        howToPlay.SetTrigger("End");
        generator.SetActive(true);
        DataManager.Instance.SaveHowToPlay(1);
    }


    public void ChangeScene()
    {
        SceneManager.LoadScene(Defs.GameOverScene);
    }
    public void MuteSoundClick()
    {
        if(DataManager.Instance.muteSound == true)
        {
            DataManager.Instance.SetSound(false);
            sound.sprite = soundOn;
            SoundManager.Instance.SetSoundVolume(10f);
        }
        else
        {
            DataManager.Instance.SetSound(true);
            sound.sprite = soundOff;
            SoundManager.Instance.SetSoundVolume(-80f);
        }
    }
    public void MuteMusicClick()
    {
        if(DataManager.Instance.muteMusic == true)
        {
            DataManager.Instance.SetMusic(false);
            music.sprite = musicOn;
            SoundManager.Instance.SetMusicVolume(0f);
        }
        else
        {
            DataManager.Instance.SetMusic(true);
            music.sprite = musicOff;
            SoundManager.Instance.SetMusicVolume(-80f);
        }
    }
}
