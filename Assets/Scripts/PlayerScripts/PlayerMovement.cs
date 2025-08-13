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
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    } 

    void Update()
    {
        // Input only (non-physics logic)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Handle sprite flip
        if (moveInput > 0)
            transform.localScale = new Vector3(2, 2, 2);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-2, 2, 2);

        // Handle jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            Jump();
        }

        // Handle dodge input
        if (Input.GetKeyDown(KeyCode.LeftShift) && dodgeCooldownTimer <= 0f && !isDodging)
        {
            StartDodge();
        }

        // Animator logic (not physics)
        anim.SetBool("run", moveInput != 0 && !isDodging);
        anim.SetBool("grounded", isGrounded());
    }

    void FixedUpdate()
    {
        // Update dodge cooldown
        if (dodgeCooldownTimer > 0f)
            dodgeCooldownTimer -= Time.fixedDeltaTime;

        if (isDodging)
        {
            dodgeTimer -= Time.fixedDeltaTime;

            float direction = Mathf.Sign(transform.localScale.x);
            rb.linearVelocity = new Vector2(direction * dodgeSpeed, rb.linearVelocity.y);

            if (dodgeTimer <= 0f)
            {
                isDodging = false;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void StartDodge()
    {
        isDodging = true;
        dodgeTimer = dodgeDuration;
        dodgeCooldownTimer = dodgeCooldownTime;
        anim.SetTrigger("dodge");
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

    
}
