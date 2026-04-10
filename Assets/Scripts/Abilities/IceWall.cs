using UnityEngine;

public class IceWall : MonoBehaviour
{
    [SerializeField] private float duration = 4f;
    void Start() => Destroy(gameObject, duration);
}