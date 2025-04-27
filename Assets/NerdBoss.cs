using System.Collections;
using UnityEngine;

public class NerdBoss : MonoBehaviour
{
    [TagField]
    public string playerTag;
    public LayerMask signLayer;

    public Rigidbody2D rb;

    public Transform[] movePoints;
    public int currentPoint = 0;
    public float moveSpeed = 6;
    public float moveSmoothing = 0.5f;
    public bool canMove = false;

    public int currentHealth = 4;
    public int maxHealth = 4;

    public GameObject[] signProjectiles;
    public GameObject target = null;
    public bool canAttack = true;
    public float cooldown = 2;
    public float projectileSpeed = 3;

    public bool battleStarted = false;

    public HealthUI healthUI;
    public GameObject nerdText;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(playerTag);
        healthUI.gameObject.SetActive(false);
        nerdText.SetActive(false);

        // To tylko gdy zacznie siê walka (na razie dla testu)
        StartBattle();
    }

    private void Update()
    {
        if (!battleStarted) return;

        if (Vector2.Distance(transform.position, movePoints[currentPoint].position) <= 0.01f * moveSpeed)
        {
            rb.linearVelocity = Vector2.zero;
            canMove = false;
        }
    }

    private void FixedUpdate()
    {
        if (!battleStarted) return;
        if (!canMove) return;
        if (movePoints.Length == 0) return;

        Vector2 dir = (movePoints[currentPoint].position - transform.position).normalized;

        Vector2 desiredVelocity = dir * moveSpeed;
        if (dir != Vector2.zero)
        {
            rb.linearVelocity += moveSpeed * Time.fixedDeltaTime * dir;
        }

        rb.linearVelocity = desiredVelocity * (1f - moveSmoothing) + rb.linearVelocity * moveSmoothing;
    }

    public void Attack()
    {
        if (!battleStarted) return;

        canAttack = false;

        // Shoot
        Vector2 direction = target.transform.position - transform.position;
        GameObject projectile = Instantiate(signProjectiles[Random.Range(0, signProjectiles.Length)], transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetVelocity(direction * projectileSpeed);

        StartCoroutine(AttackCooldown());
    }

    public void StartBattle()
    {
        battleStarted = true;
        canMove = false;
        canAttack = true;
        transform.position = movePoints[currentPoint].position;
        rb.linearVelocity = Vector2.zero;
        healthUI.gameObject.SetActive(true);
        nerdText.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        if (!battleStarted) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // DEFEATED
            Debug.Log("DEFEATED");
        }
        else
        {
            canMove = true;
            int newPoint = Random.Range(0, movePoints.Length - 1);
            currentPoint = newPoint == currentPoint ? ((currentPoint + 1) % movePoints.Length) : newPoint;
        }
    }

    public IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(cooldown);

        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!battleStarted) return;

        if (((1 << collision.gameObject.layer) & signLayer.value) != 0)
        {
            SignManager sm = collision.gameObject.GetComponent<SignManager>();
            if (!sm.flipped)
            {
                sm.Flip();
            }
        }
    }
}
