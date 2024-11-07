

using UnityEngine;

public class GameSettings : MonoBehaviour
{
    //disk after select disk
    public static int numberOfDisks = 2;

    public const int minDiskCount = 3;
    public const int maxDiskCount = 10;

    public static int unlockedLevel;

    private void Start()
    {
        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        //Debug.Log("unlockLevel at gamesettings: " + unlockedLevel);
    }

    public static void UnlockNewLevel(int newUnlockedLevel)
    {
        if (newUnlockedLevel > unlockedLevel)
        {
            unlockedLevel = newUnlockedLevel;
            PlayerPrefs.SetInt("UnlockedLevel", unlockedLevel);
            PlayerPrefs.Save();
        }
    }
}
