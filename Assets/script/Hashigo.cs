using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class LadderClimb : MonoBehaviour
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
        playerWalk = GetComponent<walk>();
        playerCollider = GetComponent<Collider2D>();

        originalGravity = rb.gravityScale;
    }

    void Update()
    {
        if (!nearLadder || currentLadder == null)
            return;

        // =========================
        // 梯子の一番上まで来たか
        // =========================

        if (isClimbing)
        {
            float ladderTop = currentLadder.bounds.max.y;

            float playerBottom =
                playerCollider.bounds.min.y;

            if (playerBottom >= ladderTop - topMargin)
            {
                ExitLadderFromTop();
                return;
            }
        }

        // =========================
        // 梯子を登り始める
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
        if (!isClimbing || Keyboard.current == null)
            return;

        float horizontal = 0f;

        // =========================
        // 左右で梯子から出る
        // =========================

        if (Keyboard.current.leftArrowKey.isPressed)
            horizontal = -1f;

        if (Keyboard.current.rightArrowKey.isPressed)
            horizontal = 1f;

        if (horizontal != 0f)
        {
            StopClimbing();

            float speed =
                playerWalk != null ? playerWalk.speed : 4f;

            rb.linearVelocity =
                new Vector2(horizontal * speed, 0f);

            return;
        }

        // =========================
        // 上下移動
        // =========================

        float vertical = 0f;

        if (Keyboard.current.upArrowKey.isPressed)
            vertical = 1f;

        if (Keyboard.current.downArrowKey.isPressed)
            vertical = -1f;

        rb.linearVelocity =
            new Vector2(
                0f,
                vertical * climbSpeed
            );
    }

    // =========================
    // 梯子を開始
    // =========================

    void StartClimbing()
    {
        isClimbing = true;

        // 重力を完全に止める
        rb.gravityScale = 0f;

        rb.linearVelocity = Vector2.zero;

        if (playerWalk != null)
            playerWalk.enabled = false;
    }

    // =========================
    // 梯子を終了
    // =========================

    void StopClimbing()
    {
        isClimbing = false;

        rb.gravityScale = originalGravity;

        rb.linearVelocity = Vector2.zero;

        if (playerWalk != null)
            playerWalk.enabled = true;
    }

    // =========================
    // 梯子の一番上から出る
    // =========================

    void ExitLadderFromTop()
    {
        if (!isClimbing || currentLadder == null)
            return;

        float ladderTop = currentLadder.bounds.max.y;

        // プレイヤーの高さ
        float playerHeight =
            playerCollider.bounds.size.y;

        // プレイヤーの中心を
        // 梯子の上端 + プレイヤー半分の高さ
        // にピッタリ合わせる
        float newY =
            ladderTop +
            playerHeight / 2f +
            0.01f;

        // =========================
        // 先にプレイヤーを正しい位置へ移動
        // =========================

        rb.position = new Vector2(
            rb.position.x,
            newY
        );

        // =========================
        // 梯子状態を解除
        // =========================

        isClimbing = false;

        // 重力を戻す
        rb.gravityScale = originalGravity;

        // 縦方向の速度を完全に0
        rb.linearVelocity = Vector2.zero;

        // 通常の移動を戻す
        if (playerWalk != null)
            playerWalk.enabled = true;

        Debug.Log("梯子の上に到着！");
    }

    // =========================
    // 梯子に入った
    // =========================

    void OnTriggerEnter2D(Collider2D other)
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

    void OnTriggerExit2D(Collider2D other)
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