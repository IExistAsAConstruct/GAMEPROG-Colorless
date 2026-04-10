using UnityEngine;

public class PlayerAnimatorSwapper : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController redAnimator;
    [SerializeField] private RuntimeAnimatorController blueAnimator;
    [SerializeField] private RuntimeAnimatorController greenAnimator;
    [SerializeField] private RuntimeAnimatorController yellowAnimator;
    [SerializeField] private RuntimeAnimatorController defaultAnimator;

    private Animator animator;

    private void Awake() => animator = GetComponent<Animator>();

    private void OnEnable()
    {
        if (ColorSystem.Instance != null)
        {
            ColorSystem.Instance.OnColorChanged += ApplyAnimator;
            ApplyAnimator(ColorSystem.Instance.CurrentColor);
        }
    }

    private void OnDisable()
    {
        if (ColorSystem.Instance != null)
            ColorSystem.Instance.OnColorChanged -= ApplyAnimator;
    }

    private void ApplyAnimator(PlayerColor color)
    {
        if (animator == null) return;

        // Snapshot current state
        float currentSpeed = animator.GetFloat("Speed");
        float currentY = animator.GetFloat("yVelocity");
        bool currentGrounded = animator.GetBool("isGrounded");
        bool currentClimbing = animator.GetBool("isClimbing");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        RuntimeAnimatorController newController = color switch
        {
            PlayerColor.Red => redAnimator,
            PlayerColor.Blue => blueAnimator,
            PlayerColor.Green => greenAnimator,
            PlayerColor.Yellow => yellowAnimator,
            _ => defaultAnimator
        };

        if (newController != null && animator.runtimeAnimatorController != newController)
        {
            animator.runtimeAnimatorController = newController;

            // Restore state so the swap is seamless
            animator.SetFloat("Speed", currentSpeed);
            animator.SetFloat("yVelocity", currentY);
            animator.SetBool("isGrounded", currentGrounded);
            animator.SetBool("isClimbing", currentClimbing);
            animator.Play(stateInfo.shortNameHash, 0, stateInfo.normalizedTime);
        }
    }
}