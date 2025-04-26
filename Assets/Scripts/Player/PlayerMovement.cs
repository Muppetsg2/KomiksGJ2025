using System.Collections;
using UnityEngine;

public delegate void OnJumpStartedEventHandler();

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isFacingRight = false;
    public float jumpPower = 10f;
    public bool isJumping = false;
    Vector2 movementInput = Vector2.zero;
    public bool isGrounded = false;

    [Header("Gravity")]
    public float baseGravity = 1f;
    public float maxFallSpeed = 5f;
    public float fallGravityMult = 1.1f;

    [Header("Attack")]
    public GameObject attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask enemiesLayer;
    public LayerMask signsLayer;
    public bool canAttack = true;
    public float attackCooldown = 2f;

    [Header("Knockback")]
    public float signKnockbackForce = 10f;
    public float knockback = 0f;
    public float knockbackSmoothTime = 1f;
    private float knockbackVelocity = 0;
    private float epsilon = 5f;

    public Rigidbody2D rb;

    public event OnJumpStartedEventHandler OnJumpStarted;
    private CapsuleCollider2D capsuleCollider;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        InputManager.Instance.OnInteractPressed += OnInteract;
        InputManager.Instance.OnAttackPressed += OnAttack;
        InputManager.Instance.OnJumpPressed += OnJump;
        InputManager.Instance.OnMove += OnMove;

        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        FlipSprite();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movementInput.x * moveSpeed + knockback, rb.linearVelocity.y);

        if (Mathf.Abs(knockback) > 0)
        {
            knockback = Mathf.SmoothDamp(knockback, 0f, ref knockbackVelocity, knockbackSmoothTime);
            if (Mathf.Abs(knockback) < epsilon)
            {
                knockback = 0f;
            }
        }

        Gravity();

        isGrounded = Physics2D.CapsuleCast(capsuleCollider.bounds.center, capsuleCollider.size, CapsuleDirection2D.Vertical, capsuleCollider.transform.rotation.eulerAngles.z, Vector2.down, 0.05f);
    }

    private void Gravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallGravityMult;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    isJumping = false;
    //}

    public IEnumerator StartAttackCooldown()
    {
        canAttack = false;

        attackPoint.GetComponent<SpriteRenderer>().color = Color.yellow;

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;

        attackPoint.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Knockback(Transform source, float knockbackForce)
    {
        Vector2 direction = new Vector2(transform.position.x - source.position.x, 0).normalized;
        knockback = direction.x * knockbackForce;
    }

    #region [ Input ]

    public void OnMove(Vector2 move) => movementInput = move;

    public void OnJump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isGrounded = false;
            OnJumpStarted?.Invoke();
        }
    }

    public void OnAttack()
    {
        if (canAttack)
        {
            Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, enemiesLayer);

            foreach (Collider2D enemyGameObject in enemy)
            {
                if (!enemyGameObject.isTrigger)
                {
                    enemyGameObject.gameObject.GetComponent<Enemy>().TakeDamage(1);
                }
            }

            Collider2D[] signs = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, signsLayer);

            foreach (Collider2D signGameObject in signs)
            {
                if(!signGameObject.gameObject.GetComponent<SignManager>().Ruin())
                {
                    Knockback(signGameObject.gameObject.transform, signKnockbackForce);
                }
            }

            StartCoroutine(StartAttackCooldown());
        }
    }

    public void OnInteract()
    {
    }

    #endregion
}
