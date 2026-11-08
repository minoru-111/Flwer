using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Hashigo : MonoBehaviour
{
    public float climbSpeed = 4f;

    // 梯子の上端からどれくらい手前で終了するか
    public float topMargin = 0.15f;

    private Rigidbody2D rb;
    private walk playerWalk;
    private Collider2D playerCollider;

    private bool nearLadder = false;
    private bool isClimbing = false;

    private float originalGravity;

    private Collider2D currentLadder;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        playerWalk =
            GetComponent<walk>();

        playerCollider =
            GetComponent<Collider2D>();

        originalGravity =
            rb.gravityScale;
    }

    void Update()
    {
        if (!nearLadder ||
            currentLadder == null)
        {
            return;
        }

        // =========================
        // 梯子の一番上
        // =========================

        if (isClimbing)
        {
            float ladderTop =
                currentLadder.bounds.max.y;

            float playerBottom =
                playerCollider.bounds.min.y;

            if (playerBottom >=
                ladderTop - topMargin)
            {
                ExitLadderFromTop();
                return;
            }
        }

        // =========================
        // 梯子に登る
        // =========================

        if (!isClimbing &&
            Keyboard.current != null &&
            (Keyboard.current.upArrowKey.isPressed ||
             Keyboard.current.downArrowKey.isPressed))
        {
            StartClimbing();
        }
    }

    void FixedUpdate()
    {
        if (!isClimbing ||
            Keyboard.current == null)
        {
            return;
        }

        // =========================
        // 左右
        // =========================

        float horizontal = 0f;

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            horizontal = -1f;
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            horizontal = 1f;
        }

        if (horizontal != 0f)
        {
            StopClimbing();

            float speed =
                playerWalk != null
                ? playerWalk.speed
                : 4f;

            rb.linearVelocity =
                new Vector2(
                    horizontal * speed,
                    0f
                );

            return;
        }

        // =========================
        // 上下
        // =========================

        float vertical = 0f;

        if (Keyboard.current.upArrowKey.isPressed)
        {
            vertical = 1f;
        }

        if (Keyboard.current.downArrowKey.isPressed)
        {
            vertical = -1f;
        }

        rb.linearVelocity =
            new Vector2(
                0f,
                vertical * climbSpeed
            );
    }

    // =========================
    // 梯子開始
    // =========================

    void StartClimbing()
    {
        isClimbing = true;

        rb.gravityScale = 0f;

        rb.linearVelocity =
            Vector2.zero;

        if (playerWalk != null)
        {
            playerWalk.enabled = false;
        }
    }

    // =========================
    // 梯子終了
    // =========================

    void StopClimbing()
    {
        isClimbing = false;

        rb.gravityScale =
            originalGravity;

        rb.linearVelocity =
            Vector2.zero;

        if (playerWalk != null)
        {
            playerWalk.enabled = true;
        }
    }

    // =========================
    // 梯子の一番上
    // =========================

    void ExitLadderFromTop()
    {
        if (!isClimbing ||
            currentLadder == null)
        {
            return;
        }

        float ladderTop =
            currentLadder.bounds.max.y;

        float playerHeight =
            playerCollider.bounds.size.y;

        // =========================
        // プレイヤーを梯子の上へ
        // =========================

        float newY =
            ladderTop +
            playerHeight / 2f +
            0.01f;

        rb.position =
            new Vector2(
                rb.position.x,
                newY
            );

        // =========================
        // 梯子状態を解除
        // =========================

        isClimbing = false;

        // 重力を戻す
        rb.gravityScale =
            originalGravity;

        // 速度を完全に0
        rb.linearVelocity =
            Vector2.zero;

        // 通常状態へ
        if (playerWalk != null)
        {
            playerWalk.enabled = true;
        }

        Debug.Log("梯子の上に到着！");
    }

    // =========================
    // 梯子に入った
    // =========================

    void OnTriggerEnter2D(
        Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            nearLadder = true;
            currentLadder = other;
        }
    }

    // =========================
    // 梯子から出た
    // =========================

    void OnTriggerExit2D(
        Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            if (currentLadder == other)
            {
                nearLadder = false;
                currentLadder = null;
            }

            if (isClimbing)
            {
                StopClimbing();
            }
        }
    }
}