using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 1000.0f;
    public Transform playerBody;
    public Joystick rightJoystick;

    private float XRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity + rightJoystick.Horizontal;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity + rightJoystick.Vertical;

        float mouseX = rightJoystick.Horizontal;
        float mouseY = rightJoystick.Vertical;

        XRotation -=mouseY;
        XRotation = Mathf.Clamp(XRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(XRotation, 0.0f, 0.0f);
        playerBody.Rotate(Vector3.up * mouseX);

    }
}
