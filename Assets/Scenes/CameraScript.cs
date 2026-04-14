using UnityEngine;

public class MouseLookRB : MonoBehaviour
{
    public float mouseSensitivity = 200f;
    public float smoothTime = 0.05f; // LOWER = snappier, HIGHER = smoother

    public Transform playerBody;

    float xRotation = 0f;

    float currentMouseX;
    float currentMouseY;

    float mouseXVelocity;
    float mouseYVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Raw mouse input
        float targetMouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float targetMouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Smooth the input
        currentMouseX = Mathf.SmoothDamp(currentMouseX, targetMouseX, ref mouseXVelocity, smoothTime);
        currentMouseY = Mathf.SmoothDamp(currentMouseY, targetMouseY, ref mouseYVelocity, smoothTime);

        // Vertical rotation
        xRotation -= currentMouseY * 100f;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation
        playerBody.Rotate(Vector3.up * currentMouseX * 100f);
    }
}