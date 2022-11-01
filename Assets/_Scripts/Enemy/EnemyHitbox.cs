using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    Enemy enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(enemy.lastAttack);
        // Атака
        if (enemy.readyToAttack && Time.time - enemy.lastAttack > enemy.cooldown)           // если готовы атаковать и кд готово
        {
            //Debug.Log("Attack!");
            enemy.lastAttack = Time.time;                                                   // присваиваем время атаки

            Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, enemy.attackRadius);  // создаем круг в позиции объекта с радиусом
            foreach (Collider2D enObjectBox in collidersHitbox)
            {
                if (enObjectBox == null)
                {
                    continue;
                }

                if (enObjectBox.gameObject.TryGetComponent<Player>(out Player player))                  // ищем скрипт плеера
                {
                    player.TakeDamage(enemy.attackDamage);                                              // наносим урон
                    Vector2 vec2 = (player.transform.position - transform.position).normalized;         // вычисляем вектор направления удара
                    player.rb2D.AddForce(vec2 * enemy.pushForce, ForceMode2D.Impulse);                  // даём импульс
                    enemy.animator.SetTrigger("Attack");                                                // начинаем анимацию
                    //Debug.Log("Player!");
                }
                collidersHitbox = null;                                                                 // сбрасываем все найденные объекты (на самом деле непонятно как это работает)
            }            
        }
    }
}
