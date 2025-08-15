using UnityEngine;

public class ParticleFlipper : MonoBehaviour
{
    private ParticleSystemRenderer particleRenderer; // Controls rendering of the particle system
    private Transform player;                        // Reference to the parent (player transform)

    // --- Unity lifecycle: delegate multi-line setup to helper ---
    void Start()
    {
        Initialize(); // Cache references to renderer + player
    }

    // --- Unity lifecycle: delegate multi-line per-frame logic to helper ---
    void Update()
    {
        UpdateFlip(); // Check player facing and flip particles accordingly
    }

    // --- Helpers (extracted from Start/Update) ---

    // Cache particle system renderer + assume parent is the player
    private void Initialize()
    {
        particleRenderer = GetComponent<ParticleSystemRenderer>();
        player = transform.parent; // Assumes particle system is child of the player
    }

    // Flip particle system horizontally based on player's facing direction
    private void UpdateFlip()
    {
        if (player.localScale.x < 0) // Facing left
        {
            particleRenderer.flip = new Vector3(1, 0, 0); // Flip X
        }
        else // Facing right
        {
            particleRenderer.flip = new Vector3(0, 0, 0); // No flip
        }
    }
}
