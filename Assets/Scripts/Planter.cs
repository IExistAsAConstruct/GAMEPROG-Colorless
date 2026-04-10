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
        currentVine = Instantiate(vinePrefab, transform.position, Quaternion.identity, transform);
        currentVine.transform.localScale = new Vector3(1f, vineHeight, 1f);
    }

    public void ClearVine()
    {
        if (currentVine != null) Destroy(currentVine);
        HasVine = false;
    }
}
