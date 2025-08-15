using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 1;
    private int currentHealth;
    private Slider healthBar; // Now private since we'll find it automatically

    private Rigidbody2D rb;
    private Animator anim;

    [Header("Drop Settings")]
    public GameObject[] alphabetDropPrefabs;

    // --- Unity lifecycle: delegate multi-line work to helpers ---
    void Start()
    {
        Initialize(); // sets components, currentHealth, and health bar state
    }

    // Apply damage, update UI, and trigger death/hurt as appropriate
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Update health bar value
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
            anim.SetTrigger("hurt");
        }
    }

    // Handle death animation, physics/collider disabling, and delayed cleanup
    private void Die()
    {
        if (healthBar != null)
            healthBar.gameObject.SetActive(false);

        anim.SetTrigger("die");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(DelayedDeath());
    }

    // Wait briefly, optionally drop a random alphabet prefab, then destroy enemy
    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(1.0f);

        if (alphabetDropPrefabs != null && alphabetDropPrefabs.Length > 0)
        {
            int index = Random.Range(0, alphabetDropPrefabs.Length);
            Instantiate(alphabetDropPrefabs[index], transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    // --- Helpers (extracted from Start; functionality unchanged) ---

    // Orchestrates initial setup
    private void Initialize()
    {
        InitializeComponents();
        InitializeHealthValues();
        InitializeHealthBar();
    }

    // Cache required components (Rigidbody2D, Animator, Slider)
    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Automatically find the health bar in children (active or inactive)
        healthBar = GetComponentInChildren<Slider>(true);
    }

    // Set starting health numbers
    private void InitializeHealthValues()
    {
        currentHealth = maxHealth;
    }

    // Configure the health bar's visibility, range, and starting value
    private void InitializeHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }
}
