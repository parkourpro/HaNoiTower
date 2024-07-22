using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastExample : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Kiểm tra khi nhấn chuột trái
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Debug.Log("Raycast fired");

            if (Physics.Raycast(ray, out hit))
            {
                // Kiểm tra nếu đối tượng trúng raycast có tên là "InteractableObject"
                if (hit.collider.gameObject.name == "InteractableObject")
                {
                    //Debug.Log("Hit object: " + hit.collider.gameObject.name);

                    // Gọi một hàm trong đối tượng bị raycast
                    hit.collider.gameObject.SendMessage("OnRaycastHit");
                }
            }
            //else
            //{
            //    Debug.Log("Raycast did not hit anything");
            //}
        }
    }
}
