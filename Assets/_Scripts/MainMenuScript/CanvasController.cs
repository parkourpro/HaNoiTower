using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Button startButton;
    public Button instructionButton;
    public Button exitButton;

    public GameObject howToPlayPanel;
    public Button howToPlayButton;
    public Button closeHowToPlayPanel;

    private ButtonClickSound buttonClickSound;
    private void Start()
    {
        howToPlayPanel.SetActive(false);
        startButton.onClick.AddListener(OnStartButtonClick);
        instructionButton.onClick.AddListener(OnInstructionButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
        howToPlayButton.onClick.AddListener(OnHowToPlayButtonClick);
        closeHowToPlayPanel.onClick.AddListener(OnCloseHowToPlayPanelClick);

        buttonClickSound = closeHowToPlayPanel.GetComponent<ButtonClickSound>();
    }


    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("SelectDisks");
    }
    public void OnInstructionButtonClick()
    {
        SceneManager.LoadScene("InstructionScene");
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }

    public void OnHowToPlayButtonClick()
    {
        howToPlayPanel.SetActive(true);
    }

    public void OnCloseHowToPlayPanelClick()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.PlayButtonClickSound();
            StartCoroutine(DelayedDeactivatePanel(howToPlayPanel, buttonClickSound.buttonClickSound.length));
        }
        else
        {
            howToPlayPanel.SetActive(false);
        }
    }

    private IEnumerator DelayedDeactivatePanel(GameObject panel, float delay)
    {
        yield return new WaitForSeconds(delay);
        panel.SetActive(false);
    }




    //public void MoveToSelectDisk()
    //{
    //    SceneManager.LoadScene("SelectDisks");
    //}
    //public void OpenInstruction()
    //{
    //    SceneManager.LoadScene("InstructionScene");

    //}

    //public void QuitGame()
    //{
    //    Application.Quit();
    //}
}
