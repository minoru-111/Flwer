using UnityEngine;

// Keep a tiled, distant backdrop behind the level at any Game view aspect ratio.
[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class VineBackground : MonoBehaviour
{
    [Range(0f, 1f)] public float parallax = 0.15f;

    void LateUpdate()
    {
        Camera view = GetComponentInParent<Camera>();
        SpriteRenderer backdrop = GetComponent<SpriteRenderer>();
        if (view == null || !view.orthographic || backdrop.sprite == null) return;

        Vector3 parentScale = transform.parent.lossyScale;
        if (Mathf.Abs(parentScale.x) < 0.0001f || Mathf.Abs(parentScale.y) < 0.0001f) return;
        transform.localScale = new Vector3(1f / parentScale.x, 1f / parentScale.y, 1f);

        Vector2 tile = backdrop.sprite.bounds.size;
        float height = view.orthographicSize * 2f;
        float width = height * view.aspect;
        backdrop.drawMode = SpriteDrawMode.Tiled;
        backdrop.tileMode = SpriteTileMode.Continuous;
        backdrop.sortingOrder = -100;
        // Whole tiles and an extra tile on each edge prevent gaps while scrolling.
        backdrop.size = new Vector2(
            (Mathf.Ceil(width / tile.x) + 2f) * tile.x,
            (Mathf.Ceil(height / tile.y) + 2f) * tile.y);
        Vector3 position = view.transform.position;
        transform.position = position + view.transform.forward * 20f
            - view.transform.right * Mathf.Repeat(position.x * parallax, tile.x)
            - view.transform.up * Mathf.Repeat(position.y * parallax, tile.y);
    }
}
