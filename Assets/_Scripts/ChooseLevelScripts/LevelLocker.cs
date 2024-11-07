using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Nếu bạn dùng TextMeshPro

public class LevelLocker : MonoBehaviour
{

    IEnumerator Start()
    {
        // Chờ cho đến khi unlockedLevel được khởi tạo
        yield return new WaitUntil(() => GameSettings.unlockedLevel > 0);
        int childCount = transform.childCount;

        //Debug.Log(GameSettings.unlockedLevel);

        for (int i = 0; i < childCount; i++)
        {
            GameObject button = transform.GetChild(i).gameObject;
            int level = i + 1;

            if (level <= GameSettings.unlockedLevel)
            {

                UnlockButton(button);
            }
            else
            {
                LockButton(button);
            }
        }
    }


    // Hàm để khóa button
    void LockButton(GameObject button)
    {
        // Tìm các gray có tag "Lock"
        Image[] lockImages = button.GetComponentsInChildren<Image>(true);

        foreach (Image img in lockImages)
        {
            if (img.CompareTag("Lock"))
            {
                //Debug.Log("Lock gray");
                img.gameObject.SetActive(true); // Bật hình khóa và hình sẫm màu
            }
        }

        // Vô hiệu hóa chức năng bấm của button
        //button.GetComponent<Button>().interactable = false;
    }

    // Hàm để mở khóa button
    void UnlockButton(GameObject button)
    {
        // Tìm các gray có tag "Lock"
        Image[] lockImages = button.GetComponentsInChildren<Image>();

        foreach (Image img in lockImages)
        {
            if (img.CompareTag("Lock"))
            {
                //Debug.Log("Unlock gray");
                img.gameObject.SetActive(false); // Tắt hình khóa và hình sẫm màu
            }
        }

        // Kích hoạt chức năng bấm của button
        //button.GetComponent<Button>().interactable = true;
    }
}
