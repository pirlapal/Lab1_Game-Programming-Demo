using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float height = 2.5f;
    public float mouseSensitivity = 0.3f;
    public float smoothSpeed = 10f;

    private float currentYaw;
    private float currentPitch = 15f;
    private bool cameraActive = true;

    void Start()
    {
        LockCursor();

        if (target != null)
        {
            currentYaw = target.eulerAngles.y;
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UnlockCursor();
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && !cameraActive)
        {
            LockCursor();
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (cameraActive && Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            currentYaw += mouseDelta.x * mouseSensitivity;
            currentPitch -= mouseDelta.y * mouseSensitivity;
            currentPitch = Mathf.Clamp(currentPitch, -10f, 60f);
        }

        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);
        Vector3 desiredPosition = target.position + Vector3.up * height + offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1f);
    }

    void LockCursor()
    {
        cameraActive = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        cameraActive = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
