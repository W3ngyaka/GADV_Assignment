using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target & Settings")]
    public Transform player;               // The player transform to follow
    public float smoothSpeed = 0.125f;     // Interpolation factor for smooth movement
    public Vector3 offset = new Vector3(0, 0, -10); // Keep camera behind player in 2D

    void LateUpdate()
    {
        // Target position for the camera (player position + offset)
        Vector3 desiredPosition = player.position + offset;
        // Smoothly interpolate from current position to desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Apply final position to camera
        transform.position = smoothedPosition;
    }
}