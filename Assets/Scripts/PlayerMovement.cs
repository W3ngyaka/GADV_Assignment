using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;       // How fast the player moves left/right
    public float jumpForce = 10f;      // How high the player jumps
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCollider;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Horizontal movement
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Flip player when moving left right
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }


        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            Jump();
        }

        // set animator parameters
        anim.SetBool("run", moveInput != 0);
        anim.SetBool("grounded", isGrounded());

        print(onWall());
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        anim.SetTrigger("jump");
    }

    
}