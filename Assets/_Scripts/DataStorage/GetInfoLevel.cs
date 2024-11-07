using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInfoLevel : MonoBehaviour
{


    public static float GetBestTime(int level)
    {
        float bestTime;
        List<LevelData> allLevelData = SaveManager.LoadGameData();

        // Tìm dữ liệu của level hiện tại
        LevelData levelData = allLevelData.Find(ld => ld.level == level);

        if (levelData != null)
        {
            bestTime = levelData.bestTime;
            //Debug.Log("Best time for level " + level + " is: " + bestTime);
        }
        else
        {
            //Debug.LogWarning("level " + level + " not found in saved data.");
            bestTime = 0;
        }
        return bestTime;
    }

    public static int GetStar(int level)
    {
        int star;
        List<LevelData> allLevelData = SaveManager.LoadGameData();

        // Tìm dữ liệu của level hiện tại
        LevelData levelData = allLevelData.Find(ld => ld.level == level);

        if (levelData != null) {
            star = levelData.stars;
            //Debug.Log("Best time for level " + level + " is: " + bestTime);
        }
        else
        {
            //Debug.LogWarning("level " + level + " not found in saved data.");
            star = 0;
        }
        return star;
    }
}
