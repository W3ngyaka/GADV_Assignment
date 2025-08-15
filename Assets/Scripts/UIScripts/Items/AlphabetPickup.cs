using UnityEngine;

public class AlphabetPickup : MonoBehaviour
{
    // Letter value this pickup represents (set on the prefab in the Inspector)
    public char letter = 'A'; // Assign in prefab

    // Audio clip played when the pickup is collected (assign in Inspector)
    public AudioClip pickupSound; // Assign audio clip in Inspector

    // Sprite used for this pickup (auto-fetched from the SpriteRenderer on Start)
    private Sprite sprite; // Automatically use sprite from Image

    // ---- Unity lifecycle ----
    private void Start()
    {
        // Cache the sprite from the SpriteRenderer (if present)
        sprite = GetComponent<SpriteRenderer>()?.sprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only react when the colliding object is tagged "Player"
        if (other.CompareTag("Player"))
        {
            // Try to get the PlayerInventory from the player object
            PlayerInventory inv = other.GetComponent<PlayerInventory>();
            if (inv != null)
            {
                // Add the letter and its sprite to the player's inventory
                inv.AddLetter(letter, sprite);
            }

            // Play sound at the pickup's position (Unity creates a temporary AudioSource internally)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Destroy this pickup after it has been collected
            Destroy(gameObject);
        }
    }
}
