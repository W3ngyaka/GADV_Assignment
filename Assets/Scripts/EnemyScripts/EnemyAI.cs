using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Target Settings")]
    public float moveSpeed = 3f;
    [SerializeField] private GameObject player; // Made private with serialized field
    public float chaseRange = 5f;
    public float stopDistance = 1f;

    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.7f;
    public int attackDamage = 1;
    public LayerMask playerLayer;
    public float verticalTolerance = 1f;
    public float attackCooldown = 1f;

    private float attackTimer = 0f;
    private Rigidbody2D rb;
    private Animator anim;

    // --- Unity lifecycle (tick-like): delegate multi-line work to helpers ---
    void Start()
    {
        Initialize(); // cache components and auto-assign player if needed
    }

    void FixedUpdate()
    {
        UpdateAI(); // movement + attack decisions on physics tick
    }

    // Called by Animation Event (kept inline per your rule)
    public void DealDamage()
    {
        // Apply damage to any Player hit within attack range
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
        }
    }

    // Editor-only gizmo to visualize attack range (kept inline)
    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }

    // Public method to assign player (for spawner)
    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
    }

    // --- Helpers (extracted from lifecycle methods; functionality unchanged) ---

    // Cache components and ensure we have a player reference
    private void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Auto-assign player if not set in inspector
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Debug.LogWarning("No GameObject with 'Player' tag found!", this);
            }
        }
    }

    // Core AI loop for chasing, stopping, and triggering attacks
    private void UpdateAI()
    {
        if (!player) return;

        Vector3 playerPos = player.transform.position;
        float horizontalDistance = Mathf.Abs(playerPos.x - transform.position.x);
        float verticalDistance = Mathf.Abs(playerPos.y - transform.position.y);

        // countdown to next allowed attack
        attackTimer -= Time.fixedDeltaTime;

        // Only engage if within horizontal chase range and acceptable vertical offset
        if (horizontalDistance <= chaseRange && verticalDistance <= verticalTolerance)
        {
            // Move toward player until within stop distance
            if (horizontalDistance > stopDistance)
            {
                float directionX = Mathf.Sign(playerPos.x - transform.position.x);

                // horizontal chase (preserve current vertical velocity)
                rb.linearVelocity = new Vector2(directionX * moveSpeed, rb.linearVelocity.y);

                // Face target (local scale flip)
                transform.localScale = new Vector3(directionX > 0 ? -3 : 3, 3, 3);

                anim.SetBool("isMoving", true);
            }
            else
            {
                // Stop moving when close enough
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                anim.SetBool("isMoving", false);

                // Trigger attack if in range and cooldown elapsed
                if (horizontalDistance <= attackRange && attackTimer <= 0f)
                {
                    anim.SetTrigger("attack");
                    attackTimer = attackCooldown;
                }
            }
        }
        else
        {
            // Idle when out of chase range/aligned window
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetBool("isMoving", false);
        }
    }
}
