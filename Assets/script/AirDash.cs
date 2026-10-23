
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class AirDash : MonoBehaviour
{
    public float dashSpeed = 15f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private walk playerWalk;
    private SpriteRenderer sr;

    private bool canDash;
    private bool isDashing;
    private float dashTimer;
    private float cooldownTimer;
    private float dashDirection = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerWalk = GetComponent<walk>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        canDash = AbilityManager.Instance != null &&
                  AbilityManager.Instance.hasAirDash;

        if (!canDash)
            return;

        // クールタイムを減らす
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        // Zキーでダッシュ
        if (!isDashing &&
            cooldownTimer <= 0f &&
            Keyboard.current != null &&
            Keyboard.current.zKey.wasPressedThisFrame)
        {
            StartDash();
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
                EndDash();
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = new Vector2(
                dashDirection * dashSpeed,
                0f
            );
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;

        // キャラクターの向きにダッシュ
        dashDirection = sr != null && sr.flipX ? -1f : 1f;

        // 通常の移動・ジャンプ処理を一時停止
        if (playerWalk != null)
            playerWalk.enabled = false;

        rb.linearVelocity = new Vector2(
            dashDirection * dashSpeed,
            0f
        );

        Debug.Log("ダッシュ！");
    }

    void EndDash()
    {
        isDashing = false;

        if (playerWalk != null)
            playerWalk.enabled = true;
    }
}
