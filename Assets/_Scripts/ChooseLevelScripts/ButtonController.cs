using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public Button playButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
    }
    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SetPlayButtonActive()
    {
        playButton.interactable = true;
    }
    public void SetPlayButtonUnActive()
    {
        playButton.interactable = false;
    }
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
