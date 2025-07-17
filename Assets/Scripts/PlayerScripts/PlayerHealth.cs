using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"{gameObject.name} HP: {currentHealth}");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} HP: {currentHealth}"); // Print current HP

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("die");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false; // Optional: stops further physics
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f); // Delay to let death animation play
    }
}
