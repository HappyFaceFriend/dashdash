using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public Animator canvasAnimator;
    public Animator playerAnim;
    public CharacterChange characterChange;
    public int character;

    public Animator speedPlus;
    public Button left;
    public Button right;
    public GameObject lockFrame;
    public Text lockText;
    
    int selectedCharacter;
    int highScore;
    public void HighScore0()
    {
        highScore = 0;
        DataManager.Instance.SetHighScore(0);
        DataManager.Instance.SetHighScore(0);
    }
    public void HighScore1000()
    {
        highScore = 2000;
        DataManager.Instance.SetHighScore(2000);
        DataManager.Instance.SetHighScore(2000);
    }
    void Awake()
    {
        character = DataManager.Instance.character;
        selectedCharacter = character;
        highScore = DataManager.Instance.highScore;
        SetCharacter(character);
            speedPlus.gameObject.SetActive(false);
    }
    void SetCharacter(int character)
    {
        if(character < 1 || character > 5)
            return;
        this.character = character;
        if(character == 1)
            left.interactable = false;
        else if(character == 5)
            right.interactable = false;
        else
        {
            left.interactable = true;
            right.interactable = true;
        }
        if(character < 4)
            speedPlus.gameObject.SetActive(false);
        else
        {
            speedPlus.gameObject.SetActive(true);
            speedPlus.Play("speed_normal", -1, 0f);
        }
        if(highScore >= Defs.limits[character-1])
        {
            selectedCharacter = character;
            lockFrame.SetActive(false);
        }
        else
        {
            lockText.text = ""+Defs.limits[character-1];
            lockFrame.SetActive(true);
        }
        characterChange.ChangeImage(character);
    }
    public void RightButton()
    {
        SetCharacter(character+1);
    }
    public void LeftButton()
    {
        SetCharacter(character-1);
    }
    public void StartScene()
    {
        if(character > 3)
        {
            speedPlus.gameObject.SetActive(true);
            speedPlus.Play("speed_normal", -1, 0f);
        }
    }
    public void EndScene()
    {
        canvasAnimator.SetTrigger("EndScene");
    }
    public void SwitchScene()
    {
        DataManager.Instance.character = selectedCharacter;
        SceneManager.LoadScene(Defs.GameScene);
    }
}
