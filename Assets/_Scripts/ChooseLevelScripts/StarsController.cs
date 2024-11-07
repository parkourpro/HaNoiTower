using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsController : MonoBehaviour
{
    public Sprite starYellow; // Sprite ngôi sao màu vàng
    public Sprite starBlack;  // Sprite ngôi sao màu đen


    void Start()
    {
        // Lấy tất cả các button con từ object Content
        Button[] levelButtons = GetComponentsInChildren<Button>();

        // Duyệt qua từng button level
        for (int i = 0; i < levelButtons.Length; i++)
        {
            // Gán index level (nếu bắt đầu từ level 1 thì i + 1)
            int level = i + 1;

            // Lấy số lượng sao vàng từ GetInfoLevel
            int stars = GetInfoLevel.GetStar(level);

            // Tìm các gray con có tag "Star" bên trong button
            Image[] starImages = levelButtons[i].GetComponentsInChildren<Image>();

            // Đặt màu cho các ngôi sao
            int starCount = 0;
            foreach (Image starImage in starImages)
            {
                // Chỉ xử lý những gray có tag là "Star"
                if (starImage.CompareTag("Star"))
                {
                     starImage.color = Color.white; // Gán màu trắng sáng cho ngôi sao vàng
                    if (starCount < stars)  
                    {
                        // Gán sprite vàng và đổi màu thành trắng sáng
                        starImage.sprite = starYellow;
                    }
                    else
                    {
                        // Gán sprite đen và màu mặc định
                        starImage.sprite = starBlack;
                    }
                    starCount++;
                }

            }
        }
    }
}
