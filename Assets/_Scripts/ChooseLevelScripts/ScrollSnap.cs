using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSnap : MonoBehaviour
{
    public RectTransform centerPanel; // Vị trí giữa của Panel
    public RectTransform content;
    public float snapSpeed = 30f; // Tốc độ cuộn về vị trí giữa
    public ScrollRect scrollRect; // Tham chiếu đến ScrollRect để kiểm tra vận tốc , kéo object được gắn ScrollRect vào đây
    public float stopThreshold = 100f; // Ngưỡng để xác định khi nào content dừng lại hoàn toàn
    public float scaleFactor = 1.2f; // Hệ số phóng to
    public float normalScale = 1f; // Kích thước bình thường của button

    private RectTransform[] buttons; // Mảng các nút trong Content
    private bool dragging = false; // Kiểm tra trạng thái kéo
    private bool autoScrolling = false; // Cờ kiểm tra có đang tự động cuộn không
    private Vector3 centerPos; // Tọa độ của vị trí giữa panel
    private int chooseButtonIndex = -1;

    public ButtonController buttonController;

    private void Start()
    {
        //yield return new WaitUntil(() => GameSettings.unlockedLevel > 0);
        int childCount = gameObject.transform.childCount; // Đếm số button (con của content)
        buttons = new RectTransform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            buttons[i] = gameObject.transform.GetChild(i).GetComponent<RectTransform>();
        }

        centerPos = centerPanel.position;
        //RectTransform buttonRectTransform = buttons[GameSettings.unlockedLevel].GetComponent<RectTransform>();
        //Debug.Log("buttonRectTransform: " + buttonRectTransform);
    }

    private void Update()
    {
        // Kiểm tra nếu đang kéo
        if (Input.GetMouseButton(0))
        {
            dragging = true;
            autoScrolling = false;
        }
        else
        {
            dragging = false;
        }

        if (!dragging && !autoScrolling)
        {
            // Khi thả chuột, đợi cho content dừng hẳn (vận tốc gần bằng 0) trước khi bắt đầu auto-scroll
            if (Mathf.Abs(scrollRect.velocity.x) < stopThreshold)
            {
                autoScrolling = true;
            }
        }

        if (autoScrolling)
        {
            // Tìm button gần trung tâm nhất
            int minDistanceIndex = GetNearestButtonIndex();
            if (minDistanceIndex != chooseButtonIndex)
            {
                chooseButtonIndex = minDistanceIndex;
                //Debug.Log("Button: " + (chooseButtonIndex + 1) + " is choosing");
                if (chooseButtonIndex + 1 > GameSettings.unlockedLevel)
                {
                    buttonController.SetPlayButtonUnActive();
                }
                else
                {
                    buttonController.SetPlayButtonActive();
                }
                GameSettings.numberOfDisks = chooseButtonIndex + 1;
            }

            // Gọi hàm dịch chuyển content
            MoveContentToButton(minDistanceIndex);
        }

        // Phóng to button gần nhất và thu nhỏ các button khác
        ScaleButtons();
    }

    // Hàm tìm nút gần vị trí trung tâm nhất
    private int GetNearestButtonIndex()
    {
        float minDistance = float.MaxValue;
        int minDistanceIndex = -1;

        for (int i = 0; i < buttons.Length; i++)
        {
            float distance = Mathf.Abs(centerPos.x - buttons[i].position.x);
            if (distance < minDistance)
            {
                minDistance = distance;
                minDistanceIndex = i;
            }
        }
        return minDistanceIndex;
    }

    // Hàm dịch chuyển content về button gần nhất
    public void MoveContentToButton(int minDistanceIndex)
    {
        Vector3 nearestButtonPos = buttons[minDistanceIndex].position;
        float distanceToCenter = centerPos.x - nearestButtonPos.x;

        // Cuộn từ từ về vị trí của button đó
        content.anchoredPosition = Vector2.Lerp(content.anchoredPosition,
            new Vector2(content.anchoredPosition.x + distanceToCenter, content.anchoredPosition.y), Time.deltaTime * snapSpeed);

        // Kiểm tra nếu đã gần đúng vị trí
        if (Mathf.Abs(distanceToCenter) < 1f)
        {
            autoScrolling = false; // Dừng tự động cuộn
        }
    }

    // Phóng to button gần nhất và thu nhỏ các button khác
    private void ScaleButtons()
    {
        // Tìm button gần vị trí trung tâm nhất
        int nearestButtonIndex = GetNearestButtonIndex();

        // Điều chỉnh kích thước cho từng button
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == nearestButtonIndex)
            {
                // Phóng to button gần nhất
                buttons[i].localScale = Vector3.Lerp(buttons[i].localScale, new Vector3(scaleFactor, scaleFactor, 1), Time.deltaTime * snapSpeed);
            }
            else
            {
                // Thu nhỏ các button khác về kích thước ban đầu
                buttons[i].localScale = Vector3.Lerp(buttons[i].localScale, new Vector3(normalScale, normalScale, 1), Time.deltaTime * snapSpeed);
            }
        }
    }
}
