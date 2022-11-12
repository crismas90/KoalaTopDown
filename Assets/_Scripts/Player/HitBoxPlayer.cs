using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxPlayer : MonoBehaviour
{
    Player player;
    public WeaponHolder weaponHolder;                   // ссылка на скрипт weaponHolder (для удара)
    public LayerMask layer;                             // слои для битья

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

            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, radius, layer);     // создаем круг в позиции объекта с радиусом
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
                {
                    fighter.TakeDamage(damage);
                    Vector2 vec2 = (coll.transform.position - player.transform.position).normalized;
                    fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
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
