using UnityEngine;

public class BlueAbility : ColorAbility
{
    public GameObject iceWallPrefab;

    public override void OnSpecialAbility()
    {
        Vector3 spawnPos = transform.position + (transform.right * 2f);
        Instantiate(iceWallPrefab, spawnPos, Quaternion.identity);
    }
}