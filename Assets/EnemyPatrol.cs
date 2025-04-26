using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Enemy enemy;
    public Transform[] patrolPoints;
    public int currentPoint = 0;
    public bool isPatroling = false;

    public void StartPatrol()
    {
        isPatroling = true;
    }

    public void StopPatrol()
    {
        isPatroling = false;
    }

    void FixedUpdate()
    {
        if (!isPatroling) return;
        if (patrolPoints.Length == 0) return;
        if (enemy.canMove) return;

        float dirX = patrolPoints[currentPoint].position.x - transform.position.x;
        dirX = dirX > 0 ? 1 : (dirX < 0) ? -1 : 0;

        float desiredVelocityX = dirX * enemy.patrolSpeed;
        if (dirX != 0)
        {
            enemy.rb.linearVelocityX += dirX * enemy.patrolSpeed * Time.fixedDeltaTime;
        }

        enemy.rb.linearVelocityX = desiredVelocityX * (1f - enemy.moveSmoothing) + enemy.rb.linearVelocityX * enemy.moveSmoothing;
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) <= 0.01f * enemy.patrolSpeed)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
    }
}
