using UnityEngine;
using Unity.Cinemachine;

public class CameraPan2D : MonoBehaviour
{
    public CinemachineCamera CinemachineCamera;

    [Header("Movement Settings")]
    public float PanSpeed = 10f;
    public float EdgeSize = 20f; // pixels from screen edge

    [Header("Limits (Optional)")]
    public bool UseLimits = false;
    public Vector2 MinLimit;
    public Vector2 MaxLimit;

    private Transform camFollow;

    void Start()
    {
        if (CinemachineCamera == null)
        {
            Debug.LogError("CinemachineCamera not assigned");
            enabled = false;
            return;
        }

        // Follow this object
        camFollow = transform;
        camFollow.position = CinemachineCamera.transform.position;
        CinemachineCamera.Follow = camFollow;
    }

    void Update()
    {
        Vector3 move = Vector3.zero;

        // Keyboard movement
        float h = Input.GetAxisRaw("Horizontal"); // A/D or Arrow keys
        float v = Input.GetAxisRaw("Vertical");   // W/S or Arrow keys
        move += new Vector3(h, v, 0);

        // Mouse edge movement
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x <= EdgeSize) move.x -= 1;
        if (mousePos.x >= Screen.width - EdgeSize) move.x += 1;
        if (mousePos.y <= EdgeSize) move.y -= 1;
        if (mousePos.y >= Screen.height - EdgeSize) move.y += 1;

        // Normalize to avoid faster diagonal movement
        if (move.magnitude > 1)
            move.Normalize();

        camFollow.position += move * PanSpeed * Time.deltaTime;

        // Clamp movement if limits enabled
        if (UseLimits)
        {
            camFollow.position = new Vector3(
                Mathf.Clamp(camFollow.position.x, MinLimit.x, MaxLimit.x),
                Mathf.Clamp(camFollow.position.y, MinLimit.y, MaxLimit.y),
                camFollow.position.z
            );
        }
    }
}
