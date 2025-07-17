using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    private Animator anim;
    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
        Destroy(gameObject, 3f); // Delay to let death animation play
    }
}
