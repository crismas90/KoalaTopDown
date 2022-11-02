using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRocket : Bullet
{
    public float expRadius = 3;
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyFront))
        {
            //enemyFront.TakeDamage(damage);
            Vector2 vec2 = (collision.transform.position - GameManager.instance.player.transform.position).normalized;
            enemyFront.rb2D.AddForce(vec2 * 3, ForceMode2D.Impulse);
        }

        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, expRadius);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Enemy>(out Enemy enemy))                            // ищем скрипт енеми
            {
                enemy.TakeDamage(enemy.attackDamage);                                               // наносим урон
                Vector2 vec2 = (enemy.transform.position - transform.position).normalized;          // вычисляем вектор направления удара
                enemy.rb2D.AddForce(vec2 * enemy.pushForce, ForceMode2D.Impulse);                   // даём импульс                                                                
            }
            collidersHits = null;
        }
        base.OnTriggerEnter2D(collision);
    }
}
