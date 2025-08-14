using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundController : MonoBehaviour
{
    private float startPos;
    private float length;
    public GameObject cam;
    public float parrallaxEffect;

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float distance = cam.transform.position.x * parrallaxEffect;
        float movement = cam.transform.position.x * (1 - parrallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}