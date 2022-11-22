using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    Player player;
    WeaponHolder weaponHolder;                          // ссылка на скрипт weaponHolder (для удара)    
    Animator animator;
    public LayerMask layer;                             // слои для битья

    public string weaponName;
    public Transform hitBox;
    public int damage = 10;                             // урон
    public float pushForce = 1;                         // сила толчка
    public float radius = 1;                            // радиус
    public float cooldown = 1f;                         // перезардяка атаки
    float lastAttack;                                   // время последнего удара (для перезарядки удара)

    // Для флипа оружия
    bool needFlip;                          // нужен флип (для правильного отображения оружия)    
    bool leftFlip;                          // оружие слева
    bool rightFlip = true;                  // оружие справа

    // Треил 
    public TrailRenderer trail;

    void Start()
    {
        player = GameManager.instance.player;
        weaponHolder = GameManager.instance.player.weaponHolder;            // находим скрипт weaponHolder
        animator = GetComponentInParent<Animator>();
        
    }

    
    void Update()
    {
        if (weaponHolder.attackHitBoxStart && Time.time - lastAttack > cooldown)          // если готовы атаковать и кд готово
        {
            //Debug.Log("Attack!");
            lastAttack = Time.time;                             // присваиваем время атаки
            animator.SetTrigger("Hit");
        }

        // Флип оружия
        if (Mathf.Abs(weaponHolder.aimAngle) > 90 && rightFlip)
        {
            needFlip = true;
            leftFlip = true;
            rightFlip = false;
        }
        if (Mathf.Abs(weaponHolder.aimAngle) <= 90 && leftFlip)
        {
            needFlip = true;
            rightFlip = true;
            leftFlip = false;
        }
        if (needFlip)
        {
            //Flip();
        }
    }

    public void MeleeAttack()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(hitBox.position, radius, layer);     // создаем круг в позиции объекта с радиусом
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

    public void TrailOn(int number)
    {
        if (trail)
        {
            if (number == 1)
                trail.emitting = true;
            if (number == 0)
                trail.emitting = false;
        }

    }

    void Flip()
    {
        if (leftFlip)                                                                                   // разворот налево
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);     // поворачиваем оружие через scale
        }
        if (rightFlip)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        }
        needFlip = false;
    }



    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hitBox.position, radius);
    }
}
