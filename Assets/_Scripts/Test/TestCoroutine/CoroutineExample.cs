using System.Collections;
using UnityEngine;

public class CoroutineExample : MonoBehaviour
{

    
    public float duration = 8.0f;
    public float elapsedTime = 0f;
    public float moveSpeed = 4f;
    // Bắt đầu Coroutine
    void Start()
    {
        // Bắt đầu coroutine MoveObject
        StartCoroutine(MoveObject());
    }

    // Định nghĩa Coroutine
    IEnumerator MoveObject()
    {
        // Vị trí bắt đầu và vị trí kết thúc
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x + 10, startPosition.y, startPosition.z);


        // Trong khi chưa đến điểm đích
        while (elapsedTime < duration)
        {
            // Sử dụng easing function để tính tỷ lệ
            float t = elapsedTime / duration;
            t = t * t * (3f - 2f * t); // Easing function

            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo đối tượng ở vị trí cuối cùng
        transform.position = endPosition;
    }
}
