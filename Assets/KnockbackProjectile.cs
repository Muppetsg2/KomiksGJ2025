using UnityEngine;

public class KnockbackProjectile : Projectile
{
    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
            pm.Knockback(transform, pm.signKnockbackForce);
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
}
