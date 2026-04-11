using UnityEngine;

public class BlueAbility : ColorAbility
{
    [Header("Ice Wall (Primary)")]
    public GameObject iceWallPrefab;
    public float wallDistance = 2f;
    public float wallDuration = 5f;

    [Header("Freeze Water (Secondary)")]
    public float freezeRadius = 3f;
    public LayerMask waterLayer;

    public override void OnPrimary()
    {
        if (iceWallPrefab == null) return;
        if (animator != null) animator.SetTrigger("attack");

        Vector3 spawnDirection = playerController.IsFacingRight ? Vector3.right : Vector3.left;
        Vector3 spawnPos = transform.position + (spawnDirection * wallDistance);
        spawnPos.y = transform.position.y;

        GameObject wall = Instantiate(iceWallPrefab, spawnPos, Quaternion.identity);
        wall.tag = "IceWall";
        Destroy(wall, wallDuration);
    }

    public override void OnSecondary()
    {
        if (animator != null) animator.SetTrigger("attack");

        Collider2D[] hitWater = Physics2D.OverlapCircleAll(transform.position, freezeRadius, waterLayer);
        foreach (var obj in hitWater)
        {
            Water water = obj.GetComponent<Water>();
            if (water != null) water.Freeze();
        }
    }
}