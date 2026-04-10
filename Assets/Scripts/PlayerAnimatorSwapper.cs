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
        ColorManager.Instance.OnColorChanged += ApplyAnimator;
        ApplyAnimator(ColorManager.Instance.CurrentColor);
    }

    private void ApplyAnimator(ColorType colorType)
    {
        RuntimeAnimatorController controller = colorType switch
        {
            ColorType.Red => redAnimator,
            ColorType.Blue => blueAnimator,
            ColorType.Green => greenAnimator,
            ColorType.Yellow => yellowAnimator,
            _ => whiteAnimator
        };

        if (controller != null) animator.runtimeAnimatorController = controller;
    }
}