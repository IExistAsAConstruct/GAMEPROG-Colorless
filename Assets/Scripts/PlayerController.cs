using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float climbSpeed = 4f;
    public float defaultGravity = 3f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    private float speedMultiplier = 1f;
    private Rigidbody2D rb;
    private Animator anim;

    private bool isClimbing;
    private bool onLadder;
    private bool isGrounded;

    [HideInInspector] public bool IsFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (rb != null) rb.gravityScale = defaultGravity;
        isClimbing = false;
        onLadder = false;
    }

    private void Update()
    {
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        else
            isGrounded = false;

        float moveInput = Input.GetAxisRaw("Horizontal");
        float climbInput = Input.GetAxisRaw("Vertical");

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        if (Input.GetButtonDown("Jump") && isGrounded && !isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        rb.linearVelocity = new Vector2(moveInput * (moveSpeed * speedMultiplier), rb.linearVelocity.y);

        if (onLadder && Mathf.Abs(climbInput) > 0.1f) isClimbing = true;

        if (isClimbing) HandleClimbing(climbInput);
        else HandleWalking();

        if (moveInput > 0 && !IsFacingRight) Flip();
        else if (moveInput < 0 && IsFacingRight) Flip();
    }

    private void HandleClimbing(float climbInput)
    {
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, climbInput * climbSpeed);
        anim.SetBool("isClimbing", true);
        if (Mathf.Abs(climbInput) < 0.1f) anim.speed = 0;
        else anim.speed = 1;
    }

    private void HandleWalking()
    {
        rb.gravityScale = defaultGravity;
        anim.SetBool("isClimbing", false);
        anim.speed = 1;
    }

    // This is the function the error is looking for!
    public void SetSpeedMultiplier(float amount)
    {
        speedMultiplier = amount;
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder")) onLadder = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            onLadder = false;
            isClimbing = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}