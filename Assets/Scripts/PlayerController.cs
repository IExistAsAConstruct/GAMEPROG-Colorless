using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;
    private float speedMultiplier = 1f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayers;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private float moveInput;
    public bool IsFacingRight { get; private set; } = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundCheckRadius, groundLayers);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (moveInput > 0 && !IsFacingRight) Flip();
        else if (moveInput < 0 && IsFacingRight) Flip();

        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        float finalSpeed = moveInput * moveSpeed * speedMultiplier;
        rb.linearVelocity = new Vector2(finalSpeed, rb.linearVelocity.y);
    }

    public void SetSpeedMultiplier(float multiplier) => speedMultiplier = multiplier;

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
}