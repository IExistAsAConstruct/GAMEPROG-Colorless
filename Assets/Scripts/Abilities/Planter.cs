using UnityEngine;

public class Planter : MonoBehaviour
{
    [SerializeField] private float vineHeight = 6f;

    public bool HasVine { get; private set; }
    private GameObject currentVine;

    public void GrowVine(GameObject vinePrefab)
    {
        if (HasVine) return;

        HasVine = true;
        currentVine = Instantiate(vinePrefab, transform.position, Quaternion.identity);

        Vine vine = currentVine.GetComponent<Vine>();
        if (vine != null) vine.Grow(vineHeight);
    }

    public void ClearVine()
    {
        if (currentVine != null) Destroy(currentVine);
        HasVine = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * vineHeight);
        Gizmos.DrawWireCube(transform.position + Vector3.up * vineHeight, new Vector3(0.5f, 0.1f, 0f));
    }
}
