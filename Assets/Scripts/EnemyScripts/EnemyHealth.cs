using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 1;
    private int currentHealth;

    private Rigidbody2D rb;
    private Animator anim;

    [Header("Drop Settings")]
    public GameObject[] alphabetDropPrefabs; // Updated to array of prefabs

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
        Debug.Log($"{gameObject.name} HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("die");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(DelayedDeath());
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(1.5f);

        if (alphabetDropPrefabs != null && alphabetDropPrefabs.Length > 0)
        {
            int index = Random.Range(0, alphabetDropPrefabs.Length);
            Instantiate(alphabetDropPrefabs[index], transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
