using UnityEngine;

public delegate void OnJumpStartedEventHandler();

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isFacingRight = false;
    public float jumpPower = 5f;
    public bool isJumping = false;
    Vector2 movementInput = Vector2.zero;

    public Rigidbody2D rb;

    public event OnJumpStartedEventHandler OnJumpStarted;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        InputManager.Instance.OnInteractPressed += OnInteract;
        InputManager.Instance.OnAttackPressed += OnAttack;
        InputManager.Instance.OnJumpPressed += OnJump;
        InputManager.Instance.OnMove += OnMove;
    }

    // Update is called once per frame
    void Update()
    {
        FlipSprite();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movementInput.x * moveSpeed, rb.linearVelocity.y);
    }

    void FlipSprite()
    {
        if (isFacingRight && movementInput.x < 0f || !isFacingRight && movementInput.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
    }


    #region [ Input ]

    public void OnMove(Vector2 move) => movementInput = move;

    public void OnJump()
    {
        if (!isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isJumping = true;
            OnJumpStarted?.Invoke();
        }
    }

    public void OnAttack()
    {

    }

    public void OnInteract()
    {

    }

    #endregion
}
