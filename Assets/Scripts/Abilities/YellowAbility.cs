using UnityEngine;

public class YellowAbility : ColorAbility
{
    [Header("Movement")]
    public float speedBoostMultiplier = 1.6f;
    public float hoverFallSpeed = -0.5f;

    [Header("Electricity")]
    public GameObject electricPrefab;
    private GameObject currentElectricEffect;

    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnActivate()
    {
        base.OnActivate();
        if (playerController != null) playerController.SetSpeedMultiplier(speedBoostMultiplier);
    }

    public override void OnDeactivate()
    {
        base.OnDeactivate();
        if (playerController != null) playerController.SetSpeedMultiplier(1f);
        StopElectricity();
    }

    public override void OnPrimary()
    {
        if (currentElectricEffect == null && electricPrefab != null)
        {
            // We spawn it as a child of the player so it follows you
            currentElectricEffect = Instantiate(electricPrefab, transform.position, Quaternion.identity, transform);

            // This ensures it stays centered on the player
            currentElectricEffect.transform.localPosition = Vector3.zero;

            Invoke(nameof(StopElectricity), 1.0f);
        }
    }

    public override void OnSecondary() { }

    private void Update()
    {
        if (!enabled) return;

        bool isHoldingHover = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.R);

        if (isHoldingHover && rb != null && rb.linearVelocity.y < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, hoverFallSpeed);
            if (animator != null) animator.SetBool("isHovering", true);
        }
        else
        {
            if (animator != null) animator.SetBool("isHovering", false);
        }
    }

    private void StopElectricity()
    {
        if (currentElectricEffect != null) Destroy(currentElectricEffect);
    }
}