using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isFacingRight = false;
    public float jumpPower = 5f;
    public bool isJumping = false;
    Vector2 movementInput = Vector2.zero;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
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

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isJumping = true;
        }
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {

    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {

    }

    #endregion
}
