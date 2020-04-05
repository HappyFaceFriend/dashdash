using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource gameBgm;
    public AudioSource mainBgm;

    public AudioSource touchSound;
    public AudioSource start;

    public AudioMixer soundMixer;
    public AudioMixer musicMixer;

    public enum Effects
    {
        Touch,Start
    }
    public enum BGM { None, Main, Game}
    BGM currentBGM = BGM.None;

    void Start()
    {
        if(DataManager.Instance.muteMusic)
            SetMusicVolume(-80f);
        if(DataManager.Instance.muteSound)
            SetSoundVolume(-80f);
    }
    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("volume",volume);
    }
    public void SetSoundVolume(float volume)
    {
        soundMixer.SetFloat("volume",volume);
    }

    public void PlayBgm(BGM type)
    {
        if(currentBGM == type && type == BGM.Main)
            return;
        if(type == BGM.Game)
        {
            currentBGM = BGM.Game;
            mainBgm.Stop();
            gameBgm.Play();
        }
        else if(type == BGM.Main)
        {
            currentBGM = BGM.Main;
            StartCoroutine(FadeIn());
            mainBgm.Play();
            gameBgm.Stop();
        }
    }
    IEnumerator FadeIn()
    {
        float eTime = 0f;
        while (eTime < 1f)
        {
            mainBgm.volume = eTime * 1f;
            eTime += Time.unscaledDeltaTime;
            yield return null;
        }
        mainBgm.volume = 1f;
    }
    public void PlaySound(Effects type, float delay = 0f)
    {
        if(DataManager.Instance.muteSound)
            return;
        if(type == Effects.Touch)
            touchSound.Play();
        else if(type == Effects.Start)
            start.PlayDelayed(delay);
    }
}
