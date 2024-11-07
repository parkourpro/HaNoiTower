using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public Slider slider;  // Thanh slider sẽ được gán từ editor
    public Image[] stars;  // Mảng lưu trữ hình ảnh của các ngôi sao
    //public Sprite goldStar;  // Texture vàng
    public Sprite silverStar;  // Texture bạc

    private float currentTime = 0f; // Thời gian hiện tại
    static bool isPaused;

    private int numberOfDisks = GameSettings.numberOfDisks;
    private float threeStarThreshold;
    private float twoStarThreshold;
    private int starCount = 3;

    void Start()
    {
        isPaused = false;
        slider.maxValue = Mathf.RoundToInt(Mathf.Pow(2, numberOfDisks + 1));
        slider.value = 0f;

        // Tính toán mốc thời gian để nhận 3 sao và 2 sao
        threeStarThreshold = Mathf.RoundToInt(Mathf.Pow(2, numberOfDisks)) * 1.4f;
        twoStarThreshold = Mathf.RoundToInt(Mathf.Pow(2, numberOfDisks)) * 1.7f;
    }

    void Update()
    {
        if (!isPaused)
        {
            currentTime += Time.deltaTime;
            slider.value = currentTime;

            // Kiểm tra trạng thái ngôi sao dựa trên thời gian
            if (currentTime >= twoStarThreshold)
            {
                //stars[0].sprite = silverStar;  // Đổi ngôi sao đầu tiên thành bạc
                stars[1].sprite = silverStar;  // Đổi ngôi sao thứ hai thành bạc
                starCount = 1;
            }
            else if (currentTime >= threeStarThreshold)
            {
                starCount = 2;
                stars[0].sprite = silverStar;  // Đổi ngôi sao đầu tiên thành bạc
            }
        }
    }

    public static void TogglePauseResume()
    {
        isPaused = !isPaused;  // Đảo ngược trạng thái (pause hoặc resume)
    }

    public LevelData GetInfoWinLevel()
    {
        var roundedTime = Mathf.Round(currentTime * 100f) / 100f;
        //Debug.Log(roundedTime);
        var levelData = new LevelData
        {
            level = numberOfDisks,
            stars = starCount,
            bestTime = roundedTime
        };
        return levelData;
    }
}
