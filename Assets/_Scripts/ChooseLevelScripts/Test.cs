using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public GameObject levelPanel;
    public GameObject content;
    // Start is called before the first frame update
    void Start()
    {
        Canvas.ForceUpdateCanvases();
        RectTransform levelPanelRect = levelPanel.GetComponent<RectTransform>();
        float xMove = levelPanelRect.anchoredPosition.x;
        RectTransform contentRect = content.GetComponent<RectTransform>();


        //Debug.Log("levelPanelRect: " + levelPanelRect.anchoredPosition);
        //Debug.Log("contentRect: " + contentRect.anchoredPosition);
        float spacing = content.GetComponent<HorizontalLayoutGroup>().spacing;
        float levelWidth = content.transform.GetChild(0).GetComponent<RectTransform>().rect.width;
        int unLockLevel = GameSettings.unlockedLevel;
        float tran = (5.5f - unLockLevel) * (levelWidth+ spacing);
        Vector2 contentAnchorPos = content.GetComponent<RectTransform>().anchoredPosition;
        Vector2 newPos = new Vector2(tran + xMove, contentAnchorPos.y);
        content.GetComponent<RectTransform>().anchoredPosition = newPos;
    }

    // Update is called once per frame
    void Update()
    {

    }
}