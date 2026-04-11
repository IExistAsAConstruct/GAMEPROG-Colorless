using UnityEngine;

public class Water : MonoBehaviour
{
    public Sprite frozenSprite;
    public float freezeTime = 5f;

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

        Invoke("Unfreeze", freezeTime);
    }

    void Unfreeze()
    {
        isFrozen = false;
        sr.sprite = originalSprite;

        // Match this to the layer your BlueAbility looks for
        gameObject.layer = LayerMask.NameToLayer("Water");

        if (col != null) col.isTrigger = true;
    }
}