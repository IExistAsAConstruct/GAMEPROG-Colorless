using UnityEngine;

public class ColorAbilities : MonoBehaviour
{
    [Header("Red - Fire")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireballSpeed = 12f;
    [SerializeField] private float fireballLifetime = 3f;
    [SerializeField] private float fireJumpForce = 14f;
    private GameObject activeFireball;

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
                case PlayerColor.Red: ShootFireball(); break;
                case PlayerColor.Blue: PlaceIceWall(); break;
                case PlayerColor.Green: SpawnHelperPlant(); break;
            }
        }

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
    private void ShootFireball()
    {
        if (activeFireball != null) return;

        float dir = playerController.FacingSign;
        activeFireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        activeFireball.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(dir * fireballSpeed, 0f);
        Destroy(activeFireball, fireballLifetime);
    }

    private void FireJump()
    {
        if (!canFireJump) return;

        canFireJump = false;
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
