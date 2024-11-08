using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouselook : MonoBehaviour
{
    public float mouseSens = 300f;

    public Transform playerBody;

    float xRotation = 0;

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSens * Time.deltaTime;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
