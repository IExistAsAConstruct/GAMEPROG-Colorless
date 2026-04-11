using UnityEngine;

public class YellowAbility : ColorAbility
{
    [Header("Movement")]
    public float speedBoostMultiplier = 1.6f;
    public float hoverGravityScale = 0.1f;

    [Header("Electricity (Primary)")]
    public GameObject electricPrefab;
    private GameObject currentElectricEffect;

    private Rigidbody2D rb;
    private bool isHovering;

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
        StopHover();
        StopElectricity();
    }

    public override void OnPrimary()
    {
        if (currentElectricEffect == null && electricPrefab != null)
        {
            currentElectricEffect = Instantiate(electricPrefab, transform.position, Quaternion.identity, transform);
            currentElectricEffect.transform.localPosition = Vector3.zero;
            Invoke(nameof(StopElectricity), 1.0f);
        }
    }

    public override void OnSecondary()
    {
        StartHover();
    }

    private void Update()
    {
        if (!enabled) return;

        // Hold R to hover, release to stop
        if (isHovering && !Input.GetKey(KeyCode.R))
            StopHover();
    }

    private void StartHover()
    {
        if (isHovering) return;
        isHovering = true;
        playerController.SetGravityOverride(true);
        rb.gravityScale = hoverGravityScale;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        if (animator != null) animator.SetBool("isHovering", true);
    }

    private void StopHover()
    {
        if (!isHovering) return;
        isHovering = false;
        playerController.SetGravityOverride(false);
        if (animator != null) animator.SetBool("isHovering", false);
    }

    private void StopElectricity()
    {
        if (currentElectricEffect != null) Destroy(currentElectricEffect);
    }
}