using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    public void MoveToSelectDisk()
    {
        SceneManager.LoadScene("SelectDisks");
    }
    public void OpenInstruction()
    {
        SceneManager.LoadScene("InstructionScene");

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
