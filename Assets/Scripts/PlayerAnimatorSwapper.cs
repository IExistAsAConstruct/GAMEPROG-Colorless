using UnityEngine;

public class PlayerAnimatorSwapper : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController redAnimator;
    [SerializeField] private RuntimeAnimatorController blueAnimator;
    [SerializeField] private RuntimeAnimatorController greenAnimator;
    [SerializeField] private RuntimeAnimatorController yellowAnimator;
    [SerializeField] private RuntimeAnimatorController whiteAnimator;

    private Animator animator;

    private void Awake() => animator = GetComponent<Animator>();

    private void Start()
    {
        if (ColorManager.Instance != null)
        {
            ColorManager.Instance.OnColorChanged += ApplyAnimator;
            ApplyAnimator(ColorManager.Instance.CurrentColor);
        }
    }

    private void OnDestroy()
    {
        if (ColorManager.Instance != null)
        {
            ColorManager.Instance.OnColorChanged -= ApplyAnimator;
        }
    }

    private void ApplyAnimator(ColorType colorType)
    {
        if (animator == null) return;

        RuntimeAnimatorController newController = colorType switch
        {
            ColorType.Red => redAnimator,
            ColorType.Blue => blueAnimator,
            ColorType.Green => greenAnimator,
            ColorType.Yellow => yellowAnimator,
            _ => whiteAnimator
        };

        // THE LOCK: If the controller is already set, do nothing.
        if (animator.runtimeAnimatorController == newController) return;

        float currentSpeed = animator.GetFloat("Speed");
        float currentY = animator.GetFloat("yVelocity");
        bool currentGrounded = animator.GetBool("isGrounded");
        bool currentClimbing = animator.GetBool("isClimbing");

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float playbackTime = stateInfo.normalizedTime;
        int stateHash = stateInfo.shortNameHash;

        animator.runtimeAnimatorController = newController;

        animator.SetFloat("Speed", currentSpeed);
        animator.SetFloat("yVelocity", currentY);
        animator.SetBool("isGrounded", currentGrounded);
        animator.SetBool("isClimbing", currentClimbing);

        animator.Play(stateHash, 0, playbackTime);
    }
}