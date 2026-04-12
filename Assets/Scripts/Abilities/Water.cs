using UnityEngine;

public class Water : MonoBehaviour
{
    [Header("Visual Settings")]
    public Sprite frozenSprite;
    public Color frozenColor = new Color(0.5f, 0.8f, 1f, 1f);

    [Header("Timing")]
    [SerializeField] private float unfreezeDelay = 0.5f;

    private Sprite originalSprite;
    private Color originalColor;
    private SpriteRenderer sr;
    private BoxCollider2D col;
    private bool isFrozen = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        originalSprite = sr.sprite;
        originalColor = sr.color;
    }

    public void Freeze()
    {
        if (isFrozen) return;

        isFrozen = true;
        sr.sprite = frozenSprite;
        sr.color = frozenColor;

        gameObject.layer = LayerMask.NameToLayer("Ground");

        if (col != null)
        {
            col.isTrigger = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!isFrozen) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        Invoke(nameof(Unfreeze), unfreezeDelay);
    }

    private void Unfreeze()
    {
        isFrozen = false;

        sr.sprite = originalSprite;
        sr.color = originalColor;

        gameObject.layer = LayerMask.NameToLayer("Water");

        if (col != null)
        {
            col.isTrigger = true;
        }
    }
}