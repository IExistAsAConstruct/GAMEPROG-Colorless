using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float climbSpeed = 4f;
    [SerializeField] private float defaultGravity = 3f;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    private float speedMultiplier = 1f;
    private bool gravityOverridden = false;

    private Rigidbody2D rb;
    private Animator anim;

    // State
    private bool isClimbing;
    private bool onLadder;
    private bool isGrounded;
    public bool IsFacingRight { get; private set; } = true;
    public bool IsGrounded => isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        rb.gravityScale = defaultGravity;
    }

    private void Update()
    {
        isGrounded = groundCheck != null
            && Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moveInput = Input.GetAxisRaw("Horizontal");
        float climbInput = Input.GetAxisRaw("Vertical");

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        if (Input.GetButtonDown("Jump") && isGrounded && !isClimbing)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        rb.linearVelocity = new Vector2(moveInput * moveSpeed * speedMultiplier, rb.linearVelocity.y);

        if (onLadder && Mathf.Abs(climbInput) > 0.1f)
            isClimbing = true;

        if (isClimbing)
            HandleClimbing(climbInput);
        else if (!gravityOverridden)
            HandleWalking();

        if (moveInput > 0 && !IsFacingRight) Flip();
        else if (moveInput < 0 && IsFacingRight) Flip();
    }

    private void HandleClimbing(float climbInput)
    {
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, climbInput * climbSpeed);
        anim.SetBool("isClimbing", true);
        anim.speed = Mathf.Abs(climbInput) < 0.1f ? 0f : 1f;
    }

    private void HandleWalking()
    {
        rb.gravityScale = defaultGravity;
        anim.SetBool("isClimbing", false);
        anim.speed = 1f;
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

    // ── External API (for ColorAbilities, etc.) ─────────

    public float FacingSign => IsFacingRight ? 1f : -1f;

    public void SetSpeedMultiplier(float value) => speedMultiplier = value;

    public void SetGravityOverride(bool active) => gravityOverridden = active;

    // ── Ladder triggers ─────────────────────────────────

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