using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GreenSentry : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackInterval = 1.5f;
    public float detectionRadius = 3f;
    public int damage = 1;
    public LayerMask enemyLayer;

    [Header("Life Settings")]
    public float growTime = 1.0f;
    public float lifetime = 4f;

    [Header("Attack Visual")]
    public Color attackLineColor = Color.green;
    public float attackLineWidth = 0.05f;
    public float attackLineDuration = 0.15f;

    private float nextAttackTime;
    private bool isGrown = false;

    private void Start()
    {
        Destroy(gameObject, lifetime);
        Invoke(nameof(FinishGrowing), growTime);
    }

    private void FinishGrowing()
    {
        isGrown = true;
    }

    private void Update()
    {
        if (!isGrown) return;

        if (Time.time >= nextAttackTime)
        {
            AttackNearbyEnemies();
            nextAttackTime = Time.time + attackInterval;
        }
    }

    private void AttackNearbyEnemies()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            var target = enemy.GetComponent<EnemyBase>();
            if (target != null) target.TakeDamage(damage);
            StartCoroutine(ShowAttackLine(enemy.transform.position));
        }
    }

    private IEnumerator ShowAttackLine(Vector3 targetPos)
    {
        // Create a temporary line from sentry to target
        GameObject lineObj = new GameObject("AttackLine");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = attackLineColor;
        lr.endColor = attackLineColor;
        lr.startWidth = attackLineWidth;
        lr.endWidth = attackLineWidth;
        lr.positionCount = 2;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, targetPos);
        lr.sortingOrder = 10;

        yield return new WaitForSeconds(attackLineDuration);

        Destroy(lineObj);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}