using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;       // How fast the player moves left/right
    public float jumpForce = 10f;      // How high the player jumps

    public Transform groundCheck;      // Empty object placed under the player
    public float groundCheckRadius = 0.2f; // Radius for detecting ground
    public LayerMask groundLayer;      // Layer for ground objects

    private Rigidbody2D rb;
    private bool isGrounded;

    private bool isFacingRight = true; // Track direction player is facing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Horizontal movement
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Flip sprite depending on direction
        if (moveInput > 0 && !isFacingRight)
        {
            Flip(); // If moving right but currently facing left, flip
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip(); // If moving left but currently facing right, flip
        }

        // Check if player is on the ground using OverlapCircle
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Allow jumping only if grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // Flip the player sprite by inverting the X scale
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Draw the ground check circle in the Scene view
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
