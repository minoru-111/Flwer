
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackX = 12f;
    public float knockbackY = 5f;
    public float knockbackDuration = 0.25f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null) return;

        walk playerWalk = rb.GetComponent<walk>();
        if (playerWalk != null)
        {
            playerWalk.StartKnockback(knockbackDuration);
        }

        float direction = Mathf.Sign(
            other.bounds.center.x - transform.position.x
        );

        if (direction == 0f)
        {
            direction = other.transform.position.x >= transform.position.x
                ? 1f : -1f;
        }

        rb.linearVelocity = new Vector2(
            direction * knockbackX,
            knockbackY
        );
    }
}
