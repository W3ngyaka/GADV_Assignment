using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCollider;

    [Header("Effects")]
    public ParticleSystem walkingParticles;
    public ParticleSystem jumpingParticles;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Dodging")]
    public float dodgeSpeed = 12f;
    public float dodgeDuration = 0.2f;
    public float dodgeCooldownTime = 1f;
    private bool isDodging = false;
    private float dodgeTimer = 0f;
    private float dodgeCooldownTimer = 0f;
    private float moveInput;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    // --- Unity lifecycle: mostly one-liners; kept inline per your rule ---
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Initialize particles
        if (walkingParticles != null)
            walkingParticles.Stop();
    }

    // --- Unity lifecycle: delegate multi-line logic to a helper ---
    void Update()
    {
        HandleUpdate(); // input, sprite flip, particles, jump/dodge input, animator booleans
    }

    // --- Unity lifecycle: delegate multi-line physics logic to a helper ---
    void FixedUpdate()
    {
        HandleFixedUpdate(); // cooldowns, dodge movement, or standard horizontal movement
    }

    // Property helper to check facing direction (unchanged)
    public bool IsFacingRight
    {
        get { return transform.localScale.x > 0; }
    }

    // --- Private helpers (extracted from Update/FixedUpdate; functionality unchanged) ---

    // Collects non-physics per-frame logic: input, visuals, and animator state updates
    private void HandleUpdate()
    {
        // 1) Read horizontal input (-1, 0, 1)
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2) Flip sprite based on input direction
        if (moveInput > 0)
            transform.localScale = new Vector3(2, 2, 2);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-2, 2, 2);

        // 3) Handle walking particles (play only when moving on ground and not dodging)
        if (walkingParticles != null)
        {
            if (moveInput != 0 && isGrounded() && !isDodging)
            {
                if (!walkingParticles.isPlaying)
                    walkingParticles.Play();
            }
            else
            {
                if (walkingParticles.isPlaying)
                    walkingParticles.Stop();
            }
        }

        // 4) Jump input (space) — only when grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            Jump();
        }

        // 5) Dodge input (Left Shift) — only when off cooldown and not already dodging
        if (Input.GetKeyDown(KeyCode.LeftShift) && dodgeCooldownTimer <= 0f && !isDodging)
        {
            StartDodge();
        }

        // 6) Animator parameters (booleans only; no physics)
        anim.SetBool("run", moveInput != 0 && !isDodging);
        anim.SetBool("grounded", isGrounded());
        anim.SetBool("fall", IsFalling());
    }

    // Physics-step movement & dodge handling
    private void HandleFixedUpdate()
    {
        // Update dodge cooldown timer
        if (dodgeCooldownTimer > 0f)
            dodgeCooldownTimer -= Time.fixedDeltaTime;

        if (isDodging)
        {
            // While dodging, force horizontal velocity in facing direction
            dodgeTimer -= Time.fixedDeltaTime;

            float direction = Mathf.Sign(transform.localScale.x);
            rb.linearVelocity = new Vector2(direction * dodgeSpeed, rb.linearVelocity.y);

            // End dodge when timer expires
            if (dodgeTimer <= 0f)
            {
                isDodging = false;
            }
        }
        else
        {
            // Normal horizontal movement (preserve vertical velocity)
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    // --- Existing methods (unchanged logic/names) ---

    private void StartDodge()
    {
        isDodging = true;
        dodgeTimer = dodgeDuration;
        dodgeCooldownTimer = dodgeCooldownTime;
        anim.SetTrigger("dodge");

        // Stop particles during dodge
        if (walkingParticles != null && walkingParticles.isPlaying)
            walkingParticles.Stop();
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        anim.SetTrigger("jump");

        // Handle jumping particles
        if (jumpingParticles != null)
        {
            // Only play if not already playing
            if (!jumpingParticles.isPlaying)
            {
                jumpingParticles.Play();
            }
        }

        // Stop walking particles
        if (walkingParticles != null && walkingParticles.isPlaying)
        {
            walkingParticles.Stop();
        }
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

    private bool IsFalling()
    {
        return rb.linearVelocity.y < -0.1f && !isGrounded();
    }
}
