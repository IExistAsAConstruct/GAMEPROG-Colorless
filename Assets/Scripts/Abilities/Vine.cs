using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Vine : MonoBehaviour
{
    [SerializeField] private float colliderWidth = 0.5f;
    [SerializeField] private float fadeInDuration = 0.5f;

    private SpriteRenderer sr;
    private BoxCollider2D col;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    /// <summary>
    /// Called by Planter. Stacks vine segments upward from this position.
    /// </summary>
    public void Grow(float height)
    {
        float segmentHeight = sr.sprite.bounds.size.y;

        if (segmentHeight <= 0f)
        {
            Debug.LogWarning("Vine segment sprite has zero height.", this);
            return;
        }

        int segmentCount = Mathf.CeilToInt(height / segmentHeight);

        for (int i = 1; i < segmentCount; i++)
        {
            GameObject segment = new GameObject($"VineSegment_{i}");
            segment.transform.SetParent(transform, false);
            segment.transform.localPosition = new Vector3(0f, segmentHeight * i, 0f);

            SpriteRenderer segSr = segment.AddComponent<SpriteRenderer>();
            segSr.sprite = sr.sprite;
            segSr.color = sr.color;
            segSr.sortingLayerID = sr.sortingLayerID;
            segSr.sortingOrder = sr.sortingOrder;
        }

        float totalHeight = segmentCount * segmentHeight;
        col.size = new Vector2(colliderWidth, totalHeight);
        col.offset = new Vector2(0f, totalHeight / 2f);

        if (fadeInDuration > 0f)
            StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        SpriteRenderer[] allSprites = GetComponentsInChildren<SpriteRenderer>();

        foreach (var s in allSprites)
        {
            Color c = s.color;
            c.a = 0f;
            s.color = c;
        }

        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeInDuration);
            foreach (var s in allSprites)
            {
                Color c = s.color;
                c.a = alpha;
                s.color = c;
            }
            yield return null;
        }

        foreach (var s in allSprites)
        {
            Color c = s.color;
            c.a = 1f;
            s.color = c;
        }
    }
}