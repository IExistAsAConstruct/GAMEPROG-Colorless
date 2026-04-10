using UnityEngine;

public class ColorAbilities : MonoBehaviour
{
    [Header("Red - Fire")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float flameRange = 3f;
    [SerializeField] private float flameRadius = 1f;
    [SerializeField] private float flameDamageRate = 4f; // damage per second
    [SerializeField] private int flameDamagePerTick = 1;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float fireJumpForce = 14f;
    private float flameDamageTimer;

    [Header("Blue - Ice")]
    [SerializeField] private GameObject iceWallPrefab;
    [SerializeField] private float iceWallSpawnDistance = 2f;
    [SerializeField] private float iceWallDuration = 5f;
    [SerializeField] private GameObject iceBridgePrefab;
    [SerializeField] private float bridgeRayDistance = 10f;
    [SerializeField] private LayerMask liquidLayer;

    [Header("Green - Nature")]
    [SerializeField] private GameObject helperPlantPrefab;
    [SerializeField] private float plantSpawnRange = 3f;
    [SerializeField] private GameObject vinePrefab;
    [SerializeField] private float vineInteractRange = 3f;

    [Header("Yellow - Electric")]
    [SerializeField] private float speedMultiplier = 1.8f;
    [SerializeField] private float hoverGravityScale = 0.1f;

    private Rigidbody2D rb;
    private PlayerController playerController;
    private bool isHovering;
    private bool isSpeedBoosted;
    private bool canFireJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        ColorSystem.Instance.OnColorChanged += OnColorChanged;
    }

    private void OnDisable()
    {
        if (ColorSystem.Instance != null)
            ColorSystem.Instance.OnColorChanged -= OnColorChanged;
    }

    private void Update()
    {
        var color = ColorSystem.Instance.CurrentColor;

        if (playerController.IsGrounded)
            canFireJump = true;

        if (Input.GetMouseButtonDown(0))
        {
            switch (color)
            {
                case PlayerColor.Blue: PlaceIceWall(); break;
                case PlayerColor.Green: SpawnHelperPlant(); break;
            }
        }

        if (Input.GetMouseButton(0) && color == PlayerColor.Red)
            FlameAttack();

        if (Input.GetMouseButtonDown(1))
        {
            switch (color)
            {
                case PlayerColor.Red: FireJump(); break;
                case PlayerColor.Blue: FreezeLiquid(); break;
                case PlayerColor.Green: SpawnVine(); break;
                case PlayerColor.Yellow: StartHover(); break;
            }
        }

        if (Input.GetMouseButtonUp(1) && ColorSystem.Instance.CurrentColor == PlayerColor.Yellow)
            StopHover();
    }

    private void OnColorChanged(PlayerColor newColor)
    {
        StopHover();
        ToggleSpeedBoost(newColor == PlayerColor.Yellow);
    }

    // ── RED ──────────────────────────────────────────────
    private void FlameAttack()
    {
        flameDamageTimer -= Time.deltaTime;
        if (flameDamageTimer > 0f) return;
        flameDamageTimer = 1f / flameDamageRate;

        // Damage everything in a circle at flame range in front of player
        Vector2 center = (Vector2)firePoint.position + Vector2.right * playerController.FacingSign * flameRange * 0.5f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, flameRadius, enemyLayer);

        foreach (var hit in hits)
        {
            var enemy = hit.GetComponent<EnemyBase>();
            if (enemy != null) enemy.TakeDamage(flameDamagePerTick);
        }

        // Hook your flame VFX here (particle system, sprite animation, etc.)
    }

    private void FireJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * fireJumpForce, ForceMode2D.Impulse);
    }

    // ── BLUE ────────────────────────────────────────────
    private void PlaceIceWall()
    {
        float dir = playerController.FacingSign;
        Vector3 spawnPos = transform.position + Vector3.right * dir * iceWallSpawnDistance;
        var wall = Instantiate(iceWallPrefab, spawnPos, Quaternion.identity);
        wall.tag = "IceWall"; 
        Destroy(wall, iceWallDuration);
    }

    private void FreezeLiquid()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, bridgeRayDistance, liquidLayer);
        if (hit.collider != null)
        {
            Instantiate(iceBridgePrefab, hit.point, Quaternion.identity);
        }
    }

    // ── GREEN ───────────────────────────────────────────
    private void SpawnHelperPlant()
    {
        Vector3 spawnPos = transform.position + (Vector3)(Random.insideUnitCircle.normalized * plantSpawnRange);
        spawnPos.y = transform.position.y;
        Instantiate(helperPlantPrefab, spawnPos, Quaternion.identity);
    }

    private void SpawnVine()
    {
        Planter[] planters = FindObjectsByType<Planter>(FindObjectsSortMode.None);
        Planter nearest = null;
        float nearestDist = vineInteractRange;

        foreach (var p in planters)
        {
            if (p.HasVine) continue;
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = p;
            }
        }

        if (nearest != null)
        {
            nearest.GrowVine(vinePrefab);
        }
    }

    // ── YELLOW ──────────────────────────────────────────
    private void ToggleSpeedBoost(bool active)
    {
        if (isSpeedBoosted == active) return;
        isSpeedBoosted = active;
        if (playerController != null)
            playerController.SetSpeedMultiplier(active ? speedMultiplier : 1f);
    }

    private void StartHover()
    {
        if (isHovering) return;
        isHovering = true;
        playerController.SetGravityOverride(true);
        rb.gravityScale = hoverGravityScale;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
    }

    private void StopHover()
    {
        if (!isHovering) return;
        isHovering = false;
        playerController.SetGravityOverride(false);
    }
}
