using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform player;
    public float chaseRange = 5f;
    public float stopDistance = 1f;
    public float verticalTolerance = 1f; // How far up/down the player can be and still be detected

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // Horizontal and vertical distances separately
        float horizontalDistance = Mathf.Abs(player.position.x - transform.position.x);
        float verticalDistance = Mathf.Abs(player.position.y - transform.position.y);

        if (horizontalDistance <= chaseRange && verticalDistance <= verticalTolerance)
        {
            if (horizontalDistance > stopDistance)
            {
                // Move toward player only on the X axis
                float directionX = Mathf.Sign(player.position.x - transform.position.x);
                rb.linearVelocity = new Vector2(directionX * moveSpeed, rb.linearVelocity.y);

                // Flip sprite once, only if facing wrong way
                if (directionX > 0)
                    transform.localScale = new Vector3(-3, 3, 3); // Face right
                else
                    transform.localScale = new Vector3(3, 3, 3);  // Face left

                anim.SetBool("isMoving", true);
            }
            else
            {
                // Stop when close enough
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            // Out of range
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim.SetBool("isMoving", false);
        }
    }
}
