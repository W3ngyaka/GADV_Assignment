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

    // --- Unity lifecycle: delegate multi-line setup to helpers ---
    void Start()
    {
        Initialize();          // cache components, set starting health
        InitializeHealthBar(); // configure health bar visuals/values
    }

    // Apply damage (or heal if damage is negative), update UI, and handle death/hurt
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
        else if (damage > 0)
        {
            // Only play hurt when actually taking positive damage
            anim.SetTrigger("hurt");
        }
    }

    // Death sequence: hide bar, stop motion/physics, disable collider, destroy after delay
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

    // --- Helpers (extracted from Start; functionality unchanged) ---

    // Cache components and set initial health
    private void Initialize()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Configure health bar if assigned
    private void InitializeHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }
}
