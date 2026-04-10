using UnityEngine;

public class BlueAbility : ColorAbility
{
    [Header("Ice Wall Settings")]
    public GameObject iceWallPrefab;
    public float wallDistance = 2f;
    public float wallDuration = 5f;

    public override void OnActivate()
    {
        base.OnActivate();
    }

    public override void OnBasicAttack()
    {
        SpawnIceWall();
    }

    public override void OnSpecialAbility()
    {
        SpawnIceWall();
    }

    private void SpawnIceWall()
    {
        if (iceWallPrefab == null) return;

        if (animator != null) animator.SetTrigger("attack");

        Vector3 spawnDirection = playerController.IsFacingRight ? Vector3.right : Vector3.left;
        Vector3 spawnPos = transform.position + (spawnDirection * wallDistance);
        spawnPos.y = transform.position.y;

        GameObject wall = Instantiate(iceWallPrefab, spawnPos, Quaternion.identity);

        Destroy(wall, wallDuration);
    }
}