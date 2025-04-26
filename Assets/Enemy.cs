using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyPatrol patrol;
    public Rigidbody2D rb;
    public float patrolSpeed;
    public float followSpeed;
    public bool isRight = true;

    [TagField]
    public string playerTag;

    public GameObject followTarget;

    [Header("Health")]
    public int currentHealth;
    public int maxHealth = 1;

    [Header("Attack")]
    public GameObject attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask playerLayer;
    public bool canAttack = true;
    public bool canMove = true;
    public float prepareToAttackTime = 1f;
    public float attackTime = 1f;
    public float attackCooldown = 1f;
    public float attackDistance = 2f;

    void Start()
    {
        currentHealth = maxHealth;
        patrol.StartPatrol();
    }

    void FixedUpdate()
    {
        if (followTarget != null)
        {
            if (canMove)
            {
                Vector2 dir = new Vector2(followTarget.transform.position.x - transform.position.x, 0).normalized;

                rb.linearVelocity = dir * followSpeed + new Vector2(0, rb.linearVelocityY);
            }

            TryToAttack();
        }

        if (rb.linearVelocityX > 0 && !isRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            isRight = true;
        }
        else if (rb.linearVelocityX < 0 && isRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            isRight = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && followTarget == null)
        {
            followTarget = collision.gameObject;
            patrol.StopPatrol();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && followTarget != null)
        {
            followTarget = null;
            patrol.StartPatrol();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void TryToAttack()
    {
        if (canAttack)
        {
            float distance = Vector2.Distance(followTarget.transform.position, gameObject.transform.position);

            if (distance < attackDistance)
            {
                StartCoroutine(Attack());
            }
        }
    }

    public IEnumerator Attack()
    {
        canMove = false;
        rb.linearVelocityX = 0;

        yield return new WaitForSeconds(prepareToAttackTime);

        Collider2D[] player = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, playerLayer);

        foreach (Collider2D playerGameObject in player)
        {
            playerGameObject.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }

        yield return new WaitForSeconds(attackTime);

        canMove = true;

        StartCoroutine(StartAttackCooldown());
    }

    public IEnumerator StartAttackCooldown()
    {
        canAttack = false;

        attackPoint.GetComponent<SpriteRenderer>().color = Color.yellow;

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;

        attackPoint.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
