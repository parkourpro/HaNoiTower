using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    public GameObject winPopup;
    public GameObject pausePanel;

    public Button replayButton;
    public Button mainMenuButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button homeButton;


    void Start()
    {
        winPopup.SetActive(false);
        pausePanel.SetActive(false);

        replayButton.onClick.AddListener(OnReplayButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        pauseButton.onClick.AddListener(OnPauseButtonClick);
        resumeButton.onClick.AddListener(OnResumeButtonClick);
        homeButton.onClick.AddListener(OnHomeButtonClick);
    }

    public void ShowWinPopup()
    {
        winPopup.SetActive(true);
    }

    private void OnPauseButtonClick()
    {
        //Debug.Log("pause button clicked");
        pausePanel.SetActive(true);
        pauseButton.interactable = false;
    }

    private void OnResumeButtonClick()
    {
        pausePanel.SetActive(false);
        pauseButton.interactable = true;
    }

    private void OnHomeButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnReplayButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    private void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene("SelectDisks");
    }
}
