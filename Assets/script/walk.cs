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

    private Rigidbody2D    rbody;

    private Collider2D     col;

    private SpriteRenderer sr;

    private Vector2 moveInput = Vector2.zero;
    
    private bool jumpRequested = false;

    private bool isGrounded = false;

    private bool isJumping = false;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if (moveInput.x != 0) sr.flipX = moveInput.x < 0;
    }

    public void OnJump()
    {
        if (isGrounded && !isJumping)   jumpRequested = true;
    }

    void Update()
    {
        float myHeight = col.bounds.extents.y;
        float footy = transform.position.y - myHeight - footOffset;
        Vector2 startRay = new Vector2(transform.position.x, footy);
        isGrounded = Physics2D.Raycast(startRay, Vector2.down, checkDistance);

        if (rbody.linearVelocity.y <= 0)  isJumping = false;
    }

    void FixedUpdate()
    {
        float vx = moveInput.x * speed;
        rbody.linearVelocity = new Vector2(vx, rbody.linearVelocity.y);

        if (jumpRequested)
        {
            jumpRequested = false;
            isJumping = true;
            rbody.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
        }

        Animate();
    }

    void Animate()
    {
        bool isWalking = moveInput.x != 0 && isGrounded;

        if (isWalking && walkSprites.Length >0)
        {
            animTimer += Time.deltaTime;
            if (animTimer >= frameTime)
            {
                animTimer -= Time.deltaTime;
                animIndex = (animIndex + 1) % walkSprites.Length;
            }
            sr.sprite = walkSprites[animIndex];
        }
        else
        {
            animTimer = 0f;
            animIndex = 0;
            if (idleSptite != null) sr.sprite = idleSptite;
        }
    }
}
