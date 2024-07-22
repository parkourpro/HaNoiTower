using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectDiskController : MonoBehaviour
{
    public TextMeshProUGUI diskCountText;
    public Button increaseButton;
    public Button decreaseButton;

    private int diskCount = 3;
    private int minDiskCount = GameSettings.minDiskCount;
    private int maxDiskCount = GameSettings.maxDiskCount;
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    void Start()
    {
        UpdateDiskCountText();
    }

    public void IncreaseDiskCount()
    {
        if (diskCount < maxDiskCount)
        {
            diskCount++;
            UpdateDiskCountText();
        }
    }

    public void DecreaseDiskCount()
    {
        if (diskCount > minDiskCount)
        {
            diskCount--;
            UpdateDiskCountText();
        }
    }

    void UpdateDiskCountText()
    {
        diskCountText.text = diskCount.ToString();
    }

    public void ConfirmSelection()
    {
        GameSettings.numberOfDisks = diskCount;
        SceneManager.LoadScene("GameScene");
    }

}
