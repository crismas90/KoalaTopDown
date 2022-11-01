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
        if (enemy.readyToAttack && Time.time - enemy.lastAttack > enemy.cooldown)
        {
            //Debug.Log("Attack!");
            enemy.lastAttack = Time.time;

            Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, 3);
            foreach (Collider2D enObjectBox in collidersHitbox)
            {
                if (enObjectBox == null)
                {
                    continue;
                }

                if (enObjectBox.gameObject.TryGetComponent<Player>(out Player player))
                {
                    player.TakeDamage(enemy.attackDamage);
                    Vector2 vec2 = (player.transform.position - transform.position).normalized;
                    player.rb2D.AddForce(vec2 * enemy.pushForce, ForceMode2D.Impulse);
                    enemy.animator.SetTrigger("Attack");
                    //Debug.Log("Player!");
                }
                collidersHitbox = null;
            }            
        }
    }
}
