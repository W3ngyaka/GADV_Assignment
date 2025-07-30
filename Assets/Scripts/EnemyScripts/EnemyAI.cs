using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Target Settings")]
    public float moveSpeed = 3f;
    public GameObject player; // Changed from Transform to GameObject
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!player) return;

        Vector3 playerPos = player.transform.position;

        float horizontalDistance = Mathf.Abs(playerPos.x - transform.position.x);
        float verticalDistance = Mathf.Abs(playerPos.y - transform.position.y);
        attackTimer -= Time.fixedDeltaTime;

        if (horizontalDistance <= chaseRange && verticalDistance <= verticalTolerance)
        {
            if (horizontalDistance > stopDistance)
            {
                float directionX = Mathf.Sign(playerPos.x - transform.position.x);
                rb.linearVelocity = new Vector2(directionX * moveSpeed, rb.linearVelocity.y);

                // Flip
                transform.localScale = new Vector3(directionX > 0 ? -3 : 3, 3, 3);
                anim.SetBool("isMoving", true);
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                anim.SetBool("isMoving", false);

                if (horizontalDistance <= attackRange && attackTimer <= 0f)
                {
                    anim.SetTrigger("attack");
                    attackTimer = attackCooldown;
                }
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetBool("isMoving", false);
        }
    }

    // Called by Animation Event
    public void DealDamage()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach (Collider2D player in hitPlayers)
        {
            player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
