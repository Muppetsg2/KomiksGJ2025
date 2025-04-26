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

    void Start()
    {
        patrol.StartPatrol();
    }

    void FixedUpdate()
    {
        if (followTarget != null)
        {
            Vector2 dir = new Vector2(followTarget.transform.position.x - transform.position.x, 0).normalized;

            rb.linearVelocity = dir * followSpeed;
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
}
