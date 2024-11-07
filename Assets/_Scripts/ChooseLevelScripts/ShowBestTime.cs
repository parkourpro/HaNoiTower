using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowBestTime : MonoBehaviour
{
    public TextMeshProUGUI bestTime;
    float bestTimeFloat;
    private int level = 0;
    void Start()
    {

    }

    void Update()
    {
        if(level != GameSettings.numberOfDisks)
        {
            level = GameSettings.numberOfDisks;
            //Debug.Log("level at Update: " + level);
            bestTimeFloat = GetInfoLevel.GetBestTime(level);
            UpdateBestTimeText();
        }
    }

    void UpdateBestTimeText()
    {
        bestTime.text = bestTimeFloat.ToString();
    }
}
