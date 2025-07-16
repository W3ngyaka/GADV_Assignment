using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform player;
    public float chaseRange = 5f;
    public float stopDistance = 1f;
    public float attackRange = 1.2f;
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
        float horizontalDistance = Mathf.Abs(player.position.x - transform.position.x);
        float verticalDistance = Mathf.Abs(player.position.y - transform.position.y);
        attackTimer -= Time.fixedDeltaTime;

        if (horizontalDistance <= chaseRange && verticalDistance <= verticalTolerance)
        {
            if (horizontalDistance > stopDistance)
            {
                // Move toward player
                float directionX = Mathf.Sign(player.position.x - transform.position.x);
                rb.linearVelocity = new Vector2(directionX * moveSpeed, rb.linearVelocity.y);

                // Flip sprite
                transform.localScale = new Vector3(directionX > 0 ? -3 : 3, 3, 3);

                anim.SetBool("isMoving", true);
            }
            else
            {
                // Stop moving and attack if in attack range
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
}
