using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRocket : Bullet
{
    public float expRadius = 3;

    private void Start()
    {
        Invoke("Explosion", 1);
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemyFront))      // если попадаем ракетой прямо во врага
        {
            //enemyFront.TakeDamage(damage);
            Vector2 vec2 = (collision.transform.position - GameManager.instance.player.transform.position).normalized;
            enemyFront.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
        }
        base.OnTriggerEnter2D(collision);           // там пусто пока что
        Explosion();
    }

    public override void Explosion()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, expRadius);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Enemy>(out Enemy enemy))                            // ищем скрипт енеми
            {
                enemy.TakeDamage(damage);                                                           // наносим урон
                Vector2 vec2 = (enemy.transform.position - transform.position).normalized;          // вычисляем вектор направления удара
                enemy.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);                         // даём импульс                                                                
            }
            collidersHits = null;
        }
        CMCameraShake.Instance.ShakeCamera(3, 0.1f);            // тряска камеры
        base.Explosion();                                       // создаёт эффект и уничтожает его и объект
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireSphere(transform.position, expRadius);
    }
}
