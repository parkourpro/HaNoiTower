using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AdjustGrayImageSize : MonoBehaviour
{
    public GameObject content;
    //This script is used to adjust the gray image inside each level to fit the width of the level button.

    void Start()
    {
        Canvas.ForceUpdateCanvases();
        Button[] levelButtons = content.GetComponentsInChildren<Button>();
        //Debug.Log(levelButtons.Length);
        //RectTransform levelRectransform = levelButtons[0].GetComponent<RectTransform>();
        float levelWidth = levelButtons[0].GetComponent<RectTransform>().rect.width;
        
        for(int i = 0; i < levelButtons.Length; i++)
        {
            GameObject grayImage = levelButtons[i].transform.Find("GrayImage").gameObject;
            if (grayImage != null)
            {
                Vector2 newGraySize = grayImage.GetComponent<RectTransform>().sizeDelta;
                newGraySize.x = levelWidth;
                grayImage.GetComponent<RectTransform>().sizeDelta = newGraySize;
            }
        }
    }

    void Update()
    {
        
    }
}
