using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class walk : MonoBehaviour
{
    public float speed = 5f;
    public float jumppower = 8f;
    public float checkDistance = 0.1f;

    public Sprite[] walkSprites;
    public Sprite idleSptite;
    public float frameTime = 0.1f;
    public float animTimer = 0f;
    public int animIndex = 0;
    public float footOffset = 0.01f;

    private Rigidbody2D rbody;
    private Collider2D col;
    private SpriteRenderer sr;

    private Vector2 moveInput = Vector2.zero;
    private bool jumpRequested = false;
    private bool isGrounded = false;

    // =========================
    // ダブルジャンプ
    // =========================

    public bool canDoubleJump = false;
    private int jumpsRemaining = 1;

    // =========================
    // ノックバック
    // =========================

    public float knockbackDuration = 0.25f;
    private float knockbackTimer = 0f;

    // =========================
    // 風
    // =========================

    [HideInInspector]
    public Vector2 windForce = Vector2.zero;

    [HideInInspector]
    public float windSpeedMultiplier = 1f;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();

        // 回転しないようにする
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // 保存されているダブルジャンプを確認
        if (AbilityManager.Instance != null &&
            AbilityManager.Instance.hasDoubleJump)
        {
            UnlockDoubleJump();
        }
    }

    // =========================
    // 移動入力
    // =========================

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        // 左右を向く
        if (moveInput.x != 0)
        {
            sr.flipX = moveInput.x < 0;
        }
    }

    void Update()
    {
        // =========================
        // ジャンプ
        // =========================

        if (Keyboard.current != null &&
            (Keyboard.current.spaceKey.wasPressedThisFrame ||
             Keyboard.current.upArrowKey.wasPressedThisFrame))
        {
            RequestJump();
        }

        // =========================
        // 地面判定
        // =========================

        float myHeight = col.bounds.extents.y;

        float footy =
            transform.position.y -
            myHeight -
            footOffset;

        Vector2 startRay =
            new Vector2(transform.position.x, footy);

        isGrounded =
            Physics2D.Raycast(
                startRay,
                Vector2.down,
                checkDistance
            );

        // 地面に着いたらジャンプ回数回復
        if (isGrounded)
        {
            jumpsRemaining = canDoubleJump ? 2 : 1;
        }
    }

    // =========================
    // ジャンプ要求
    // =========================

    private void RequestJump()
    {
        // 地面にいる
        if (isGrounded && jumpsRemaining > 0)
        {
            jumpRequested = true;
        }

        // 空中でダブルジャンプ
        else if (!isGrounded &&
                 canDoubleJump &&
                 jumpsRemaining > 0)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // =========================
        // ノックバック
        // =========================

        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.fixedDeltaTime;
        }
        else
        {
            // =========================
            // 通常の横移動
            // =========================

            float vx = moveInput.x * speed;

            // =========================
            // 風の影響
            // =========================

            // まず風そのものの力を加える
            vx += windForce.x;

            // 風がある場合
            if (windForce.x != 0)
            {
                // 風と逆方向に進もうとしているか確認
                bool movingAgainstWind =
                    (windForce.x > 0 && moveInput.x < 0) ||
                    (windForce.x < 0 && moveInput.x > 0);

                // 逆風のときだけ移動速度を下げる
                if (movingAgainstWind)
                {
                    vx *= windSpeedMultiplier;
                }
            }

            // 実際に移動
            rbody.linearVelocity =
                new Vector2(
                    vx,
                    rbody.linearVelocity.y
                );
        }

        // =========================
        // ジャンプ
        // =========================

        if (jumpRequested &&
            knockbackTimer <= 0f)
        {
            jumpRequested = false;

            jumpsRemaining--;

            // 縦速度をリセット
            rbody.linearVelocity =
                new Vector2(
                    rbody.linearVelocity.x,
                    0f
                );

            // ジャンプ
            rbody.AddForce(
                Vector2.up * jumppower,
                ForceMode2D.Impulse
            );
        }
        else if (knockbackTimer > 0f)
        {
            jumpRequested = false;
        }

        // =========================
        // アニメーション
        // =========================

        Animate();
    }

    // =========================
    // ダブルジャンプ解放
    // =========================

    public void UnlockDoubleJump()
    {
        canDoubleJump = true;
        jumpsRemaining = 2;

        Debug.Log("ダブルジャンプ解放！");
    }

    // =========================
    // ノックバック開始
    // =========================

    public void StartKnockback(float duration)
    {
        knockbackTimer = duration;
    }

    // =========================
    // 風を設定
    // =========================

    public void SetWind(
        Vector2 force,
        float speedMultiplier
    )
    {
        windForce = force;
        windSpeedMultiplier = speedMultiplier;
    }

    // =========================
    // 風を解除
    // =========================

    public void ClearWind()
    {
        windForce = Vector2.zero;
        windSpeedMultiplier = 1f;
    }

    // =========================
    // 歩きアニメーション
    // =========================

    void Animate()
    {
        bool isWalking =
            moveInput.x != 0 &&
            isGrounded;

        if (isWalking &&
            walkSprites != null &&
            walkSprites.Length > 0)
        {
            animTimer += Time.deltaTime;

            if (animTimer >= frameTime)
            {
                animTimer -= frameTime;

                animIndex =
                    (animIndex + 1) %
                    walkSprites.Length;
            }

            sr.sprite =
                walkSprites[animIndex];
        }
        else
        {
            animTimer = 0f;
            animIndex = 0;

            if (idleSptite != null)
            {
                sr.sprite = idleSptite;
            }
        }
    }
}