using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float speed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [Header("Jump Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float impulse = 800.0f;
    [SerializeField] private float gravity = 2.0f;
    private bool jump;

    [Header("Animation Settings")]
    private Animator anima;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anima = GetComponent<Animator>();
        rb.gravityScale = gravity;
    }

    void Update()
    {
        float px = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        transform.Translate(px, 0.0f, 0.0f);

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            sr.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            sr.flipX = true;
        }

        //Animation
        anima.SetFloat("Move", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && OnGround(this.transform, groundCheckRadius))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    bool OnGround(Transform centerRadius, float radius)
    {
        return Physics2D.OverlapCircle(centerRadius.transform.position, radius,
        1 << LayerMask.NameToLayer("Ground"));
    }

}