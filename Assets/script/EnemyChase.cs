
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float detectionRange = 5f;
    public float loseRange = 8f;
    public float chaseSpeed = 3f;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void FixedUpdate()
    {
        if (player == null || rb == null) return;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        // 追跡開始
        if (!isChasing && distance <= detectionRange)
        {
            isChasing = true;
        }

        // 一定距離まで離れたら追跡終了
        if (isChasing && distance >= loseRange)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            float direction =
                Mathf.Sign(player.position.x - transform.position.x);

            // プレイヤーがいる方向を向く
            if (sr != null && direction != 0)
            {
                sr.flipX = direction > 0;
            }

            // 横方向に追いかける
            rb.linearVelocity = new Vector2(
                direction * chaseSpeed,
                rb.linearVelocity.y
            );
        }
        else
        {
            // 追跡していない間は停止
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, loseRange);
    }
}
