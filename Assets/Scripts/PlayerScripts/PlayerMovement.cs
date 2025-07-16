using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dodgeSpeed = 12f;
    public float dodgeDuration = 0.2f;
    public float dodgeCooldownTime = 1f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCollider;

    private bool isDodging = false;
    private float dodgeTimer = 0f;
    private float dodgeCooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Update cooldown timer
        if (dodgeCooldownTimer > 0)
            dodgeCooldownTimer -= Time.deltaTime;

        if (isDodging)
        {
            dodgeTimer -= Time.deltaTime;

            // End dodge
            if (dodgeTimer <= 0f)
                isDodging = false;
        }
        else
        {
            // Movement
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            // Flip sprite
            if (moveInput > 0)
                transform.localScale = new Vector3(2, 2, 2);
            else if (moveInput < 0)
                transform.localScale = new Vector3(-2, 2, 2);

            // Jump
            if (Input.GetKey(KeyCode.Space) && isGrounded())
                Jump();

            // Dodge
            if (Input.GetKeyDown(KeyCode.LeftShift) && dodgeCooldownTimer <= 0f)
                StartDodge();

            // Animator
            anim.SetBool("run", moveInput != 0);
            anim.SetBool("grounded", isGrounded());
        }
    }

    private void StartDodge()
    {
        isDodging = true;
        dodgeTimer = dodgeDuration;
        dodgeCooldownTimer = dodgeCooldownTime;
        anim.SetTrigger("dodge");

        float direction = Mathf.Sign(transform.localScale.x);
        rb.linearVelocity = new Vector2(direction * dodgeSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        anim.SetTrigger("jump");
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            0.1f,
            groundLayer
        );
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            new Vector2(transform.localScale.x, 0),
            0.1f,
            wallLayer
        );
        return raycastHit.collider != null;
    }
}
