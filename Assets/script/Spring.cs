
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float bouncePower = 18f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        if (rb == null)
            return;

        // 上方向に強く跳ね上げる
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            bouncePower
        );
    }
}
