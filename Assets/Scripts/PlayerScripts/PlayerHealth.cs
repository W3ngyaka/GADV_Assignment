using UnityEngine;
using UnityEngine.UI; // Add this for UI access

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public Slider healthBar; 
    private int currentHealth;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Initialize health bar
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {

            currentHealth -= damage;

            // Update health bar
            if (healthBar != null)
            {
                healthBar.value = currentHealth;
            }

            if (currentHealth <= 0)
            {
                Die();
            }
            else if (damage > 0) {
            {
                anim.SetTrigger("hurt");
            }
        }
    }

    private void Die()
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false); // Hide health bar on death
        }

        anim.SetTrigger("die");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }
}