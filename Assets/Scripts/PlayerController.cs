using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    // This allows the Yellow Ability to boost speed
    private float speedMultiplier = 1f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayers;

    // References
    private Rigidbody2D rb;
    private Animator animator;

    // State
    public bool IsFacingRight { get; private set; } = true;
    private bool isGrounded;
    private float moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 1. Get Input
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2. Ground Check
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundCheckRadius, groundLayers);

        // 3. Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 4. Flip Logic
        if (moveInput > 0 && !IsFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && IsFacingRight)
        {
            Flip();
        }

        // 5. Update Animations
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        // Physics-based movement applying the speed multiplier
        float finalSpeed = moveInput * moveSpeed * speedMultiplier;
        rb.linearVelocity = new Vector2(finalSpeed, rb.linearVelocity.y);
    }

    // This function is called by YellowAbility.cs
    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void UpdateAnimations()
    {
        if (animator != null)
        {
            animator.SetFloat("moveSpeed", Mathf.Abs(moveInput));
            animator.SetBool("isGrounded", isGrounded);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundPoint.position, groundCheckRadius);
        }
    }
}