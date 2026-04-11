using UnityEngine;

public class Water : MonoBehaviour
{
    public Sprite frozenSprite;
    [SerializeField] private float unfreezeDelay = 0.5f;

    private Sprite originalSprite;
    private SpriteRenderer sr;
    private BoxCollider2D col;
    private bool isFrozen = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        originalSprite = sr.sprite;
    }

    public void Freeze()
    {
        if (isFrozen) return;

        isFrozen = true;
        sr.sprite = frozenSprite;
        gameObject.layer = LayerMask.NameToLayer("Ground");

        if (col != null) col.isTrigger = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!isFrozen) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        // Small delay so it doesn't vanish the instant you jump
        Invoke(nameof(Unfreeze), unfreezeDelay);
    }

    void Unfreeze()
    {
        isFrozen = false;
        sr.sprite = originalSprite;
        gameObject.layer = LayerMask.NameToLayer("Water");

        if (col != null) col.isTrigger = true;
    }
}