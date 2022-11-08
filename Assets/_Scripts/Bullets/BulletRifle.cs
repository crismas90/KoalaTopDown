using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRifle : Bullet
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
            Vector2 vec2 = (collision.transform.position - GameManager.instance.player.transform.position).normalized;
            enemy.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
        }
        Explosion();
        base.OnTriggerEnter2D(collision);               // там пусто пока что
    }
    public override void Explosion()
    {
        base.Explosion();
    }
}
