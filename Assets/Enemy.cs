using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public EnemyPatrol patrol;
    public Rigidbody2D rb;
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
            Vector2 dir = new Vector2(followTarget.transform.position.x - transform.position.x, 0).normalized;

            rb.linearVelocity = dir * followSpeed;

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
                Attack();
            }
        }
    }

    public void Attack()
    {
        Collider2D[] player = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, playerLayer);

        foreach (Collider2D playerGameObject in player)
        {
            playerGameObject.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }

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
