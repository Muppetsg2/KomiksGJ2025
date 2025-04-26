using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public EnemyPatrol patrol;
    public Rigidbody2D rb;
    public bool isRight = true;

    void Start()
    {
        patrol.StartPatrol();
    }

    void FixedUpdate()
    {
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
}
