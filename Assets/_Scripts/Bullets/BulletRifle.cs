using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRifle : Bullet
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
        {
            fighter.TakeDamage(damage);
            Vector2 vec2 = (collision.transform.position - transform.position).normalized;
            fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
        }
        base.OnTriggerEnter2D(collision);               // там пусто пока что
        Explosion();
    }
    public override void Explosion()
    {
        base.Explosion();
    }
}
