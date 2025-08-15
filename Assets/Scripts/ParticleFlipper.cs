using UnityEngine;

public class ParticleFlipper : MonoBehaviour
{
    private ParticleSystemRenderer particleRenderer;
    private Transform player;

    void Start()
    {
        particleRenderer = GetComponent<ParticleSystemRenderer>();
        player = transform.parent; // Assumes particle system is child of player
    }

    void Update()
    {
        if (player.localScale.x < 0) // Facing left
            particleRenderer.flip = new Vector3(1, 0, 0);
        else // Facing right
            particleRenderer.flip = new Vector3(0, 0, 0);
    }
}