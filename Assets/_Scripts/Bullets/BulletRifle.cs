using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRifle : Bullet
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
        {
            Vector2 vec2 = (collision.transform.position - transform.position).normalized;
            fighter.TakeDamage(damage, vec2, pushForce);            
        }
        base.OnTriggerEnter2D(collision);               // ��� ����� ���� ���
        Explosion();
    }

}
