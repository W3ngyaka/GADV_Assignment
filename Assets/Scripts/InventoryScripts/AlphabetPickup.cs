using UnityEngine;

public class AlphabetPickup : MonoBehaviour
{
    public char letter = 'A'; // Assign in prefab
    public AudioClip pickupSound; // Assign audio clip in Inspector
    private Sprite sprite; // Automatically use sprite from Image

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>()?.sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inv = other.GetComponent<PlayerInventory>();
            if (inv != null)
            {
                inv.AddLetter(letter, sprite);
            }

            // Play sound at the position of the pickup (no AudioSource needed)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            Destroy(gameObject);
        }
    }
}