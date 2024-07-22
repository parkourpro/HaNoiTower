using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionUIController : MonoBehaviour
{
    public TextMeshProUGUI numberOfDisk;

    public Button back;
    public Button increaseDisk;
    public Button decreaseDisk;
    public Button pause;
    public Button resume;

    public static int diskCount = 3;
    private int minDiskCount = GameSettings.minDiskCount;
    private int maxDiskCount = GameSettings.maxDiskCount;

    public static bool isSolving = false;

    private void Start()
    {
        pause.gameObject.SetActive(false);
        back.onClick.AddListener(OnBackButtonClick);
        increaseDisk.onClick.AddListener(OnIncreaseDiskButtonClick);
        decreaseDisk.onClick.AddListener(OnDecreaseDiskButtonClick);
        pause.onClick.AddListener(OnPauseButtonClick);
        resume.onClick.AddListener(OnResumeButtonClick);

        UpdateNumberOfDisk();
    }

    private void OnBackButtonClick()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    private void OnIncreaseDiskButtonClick()
    {
        if (diskCount < maxDiskCount)
        {
            diskCount++;
            UpdateNumberOfDisk();
        }
    }

    private void OnDecreaseDiskButtonClick()
    {
        if (diskCount > minDiskCount)
        {
            diskCount--;
            UpdateNumberOfDisk();
        }
    }

    public void OnPauseButtonClick()
    {
        pause.gameObject.SetActive(false);
        resume.gameObject.SetActive(true);
        SetButtonsInteractable(true);
        isSolving = false;
    }

    private void OnResumeButtonClick()
    {
        resume.gameObject.SetActive(false);
        pause.gameObject.SetActive(true);
        SetButtonsInteractable(false);
        isSolving = true;
    }

    void UpdateNumberOfDisk()
    {
        numberOfDisk.text = diskCount.ToString();
    }

    public void SetButtonsInteractable(bool state)
    {
        increaseDisk.interactable = state;
        decreaseDisk.interactable = state;
        back.interactable = state;
    }
    public void SetResumeButtonInteractable(bool state)
    {
        resume.interactable = state;
    }
}
    