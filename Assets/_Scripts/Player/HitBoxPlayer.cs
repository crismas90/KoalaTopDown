using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxPlayer : MonoBehaviour
{
    Player player;
    public WeaponHolder weaponHolder;                   // ссылка на скрипт weaponHolder (для удара)

    public int damage = 10;                             // урон
    public float pushForce = 1;                         // сила толчка
    public float radius = 1;                            // радиус
    public float cooldown = 1f;                         // перезардяка атаки
    float lastAttack;                                   // время последнего удара (для перезарядки удара)

    void Start()
    {
        player = GameManager.instance.player;        
    }

    
    void Update()
    {
        if (weaponHolder.attackHitBoxStart && Time.time - lastAttack > cooldown)          // если готовы атаковать и кд готово
        {
            //Debug.Log("Attack!");
            lastAttack = Time.time;                             // присваиваем время атаки
            player.animator.SetTrigger("AttackHit");

            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, radius);     // создаем круг в позиции объекта с радиусом
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<Enemy>(out Enemy enemy))                            // ищем скрипт енеми
                {
                    enemy.TakeDamage(damage);                                                           // наносим урон
                    Vector2 vec2 = (enemy.transform.position - player.transform.position).normalized;          // вычисляем вектор направления удара
                    enemy.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);                         // даём импульс                                                                
                }
                collidersHits = null;
            }
        }
        
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
