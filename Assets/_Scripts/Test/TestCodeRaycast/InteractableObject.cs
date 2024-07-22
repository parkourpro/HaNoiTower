using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    void OnRaycastHit()
    {
        // Xử lý sự kiện khi đối tượng bị raycast
        Debug.Log("Object has been hit by raycast!");
        // Thực hiện các hành động khác ở đây
    }
}
