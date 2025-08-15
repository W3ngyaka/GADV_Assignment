using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Vector3 startPos;  // Stores initial position
    private float length;
    public GameObject cam;
    public float parallaxEffectX;  // Horizontal parallax effect

    private void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        // Horizontal movement with parallax
        float distanceX = cam.transform.position.x * parallaxEffectX;

        // Vertical movement follows camera directly (no parallax)
        float newY = cam.transform.position.y;

        transform.position = new Vector3(
            startPos.x + distanceX,
            newY,  // Follows camera Y directly
            startPos.z
        );

        // Infinite horizontal scrolling
        float cameraMovement = cam.transform.position.x * (1 - parallaxEffectX);
        if (cameraMovement > startPos.x + length)
        {
            startPos.x += length;
        }
        else if (cameraMovement < startPos.x - length)
        {
            startPos.x -= length;
        }
    }
}