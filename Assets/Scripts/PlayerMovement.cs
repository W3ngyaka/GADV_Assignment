using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;       // How fast the player moves left/right
    public float jumpForce = 10f;      // How high the player jumps

    private bool isGrounded;

    private Rigidbody2D rb;
    private Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Horizontal movement
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Flip player when moving left right
        if (moveInput > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }


        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // set animator parameters
        anim.SetBool("run", moveInput != 0);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) ;
        { 
            isGrounded = true;
        }
    }
}

   