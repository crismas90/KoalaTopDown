using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    Animator animator;

    public int damage;
    public float timeToActive;
    bool isSpikeActive;
    Fighter target;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SpikeActiveEvent()                   // ивент запускает инвок
    {
        Invoke("SpikeActivate", timeToActive);
    }

    void SpikeActivate()
    {
        isSpikeActive = true;
        animator.SetTrigger("SpikeHit");
        DamageAll();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
        {            
            if (isSpikeActive)
            {
                fighter.TakeDamage(damage);
            }
            //Vector2 vec2 = (collision.transform.position - GameManager.instance.player.transform.position).normalized;
            //fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
        }
    }    

    void DamageAll()
    {
        Collider2D[] collidersHits = Physics2D.OverlapBoxAll(transform.position, new Vector2 (0.5f, 0.5f), 0f);     // создаем квадрат в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                fighter.TakeDamage(damage);
/*                Vector2 vec2 = (coll.transform.position - transform.position).normalized;
                fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);*/
            }
            collidersHits = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.5f,0.5f,0f));
    }
}
