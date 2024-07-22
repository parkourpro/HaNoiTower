using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public void OnObjectClicked()
    {
        Debug.Log("Object clicked: " + gameObject.name);
        // Thêm các hành động khác tại đây, ví dụ: thay đổi màu sắc của đối tượng
        // GetComponent<Renderer>().material.color = Color.red;
    }
}
