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

        Vector2 dir = new Vector2(patrolPoints[currentPoint].position.x - transform.position.x, 0).normalized;

        enemy.rb.linearVelocity = dir * enemy.patrolSpeed;
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) <= 0.01f * enemy.patrolSpeed)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
    }
}
