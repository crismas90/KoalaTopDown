using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    Enemy enemy;
    //public WeaponHolder weaponHolder;
    public float attackRadius;                              // радиус атаки
    
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    
    void Update()
    {
        //Debug.Log(enemy.lastAttack);
        // Атака
        if (enemy.readyToAttack && Time.time - enemy.lastAttack > enemy.cooldown)           // если готовы атаковать и кд готово
        {
            //Debug.Log("Attack!");
            enemy.lastAttack = Time.time;                                                   // присваиваем время атаки

            Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, attackRadius);  // создаем круг в позиции объекта с радиусом
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
                    player.rb.AddForce(vec2 * enemy.pushForce, ForceMode2D.Impulse);                  // даём импульс
                    enemy.animator.SetTrigger("Attack");                                                // начинаем анимацию
                    //Debug.Log("Player!");
                }
                collidersHitbox = null;                                                                 // сбрасываем все найденные объекты (на самом деле непонятно как это работает)
            }            
        }        
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;        
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
