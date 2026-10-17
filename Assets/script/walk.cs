
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

    // ダブルジャンプ
    public bool canDoubleJump = false;
    private int jumpsRemaining = 1;

    public float knockbackDuration = 0.25f;
    private float knockbackTimer = 0f;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Sceneを移動しても能力を引き継ぐ
        if (AbilityManager.Instance != null &&
            AbilityManager.Instance.hasDoubleJump)
        {
            UnlockDoubleJump();
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (moveInput.x != 0)
            sr.flipX = moveInput.x < 0;
    }

    public void OnJump()
    {
        if (isGrounded && jumpsRemaining > 0)
        {
            jumpRequested = true;
        }
        else if (!isGrounded &&
                 canDoubleJump &&
                 jumpsRemaining > 0)
        {
            jumpRequested = true;
        }
    }

    void Update()
    {
        float myHeight = col.bounds.extents.y;
        float footy = transform.position.y - myHeight - footOffset;

        Vector2 startRay = new Vector2(
            transform.position.x,
            footy
        );

        isGrounded = Physics2D.Raycast(
            startRay,
            Vector2.down,
            checkDistance
        );

        if (isGrounded)
        {
            jumpsRemaining = canDoubleJump ? 2 : 1;
        }
    }

    void FixedUpdate()
    {
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.fixedDeltaTime;
        }
        else
        {
            float vx = moveInput.x * speed;

            rbody.linearVelocity = new Vector2(
                vx,
                rbody.linearVelocity.y
            );
        }

        if (jumpRequested && knockbackTimer <= 0f)
        {
            jumpRequested = false;
            jumpsRemaining--;

            // 2回目のジャンプも安定させる
            rbody.linearVelocity = new Vector2(
                rbody.linearVelocity.x,
                0f
            );

            rbody.AddForce(
                Vector2.up * jumppower,
                ForceMode2D.Impulse
            );
        }
        else if (knockbackTimer > 0f)
        {
            jumpRequested = false;
        }

        Animate();
    }

    public void UnlockDoubleJump()
    {
        canDoubleJump = true;
        jumpsRemaining = 2;

        Debug.Log("ダブルジャンプ解放！");
    }

    public void StartKnockback(float duration)
    {
        knockbackTimer = duration;
    }

    void Animate()
    {
        bool isWalking = moveInput.x != 0 && isGrounded;

        if (isWalking &&
            walkSprites != null &&
            walkSprites.Length > 0)
        {
            animTimer += Time.deltaTime;

            if (animTimer >= frameTime)
            {
                animTimer -= frameTime;
                animIndex =
                    (animIndex + 1) % walkSprites.Length;
            }

            sr.sprite = walkSprites[animIndex];
        }
        else
        {
            animTimer = 0f;
            animIndex = 0;

            if (idleSptite != null)
                sr.sprite = idleSptite;
        }
    }
}
