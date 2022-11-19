using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    Animator animator;

    public bool isSpikeWork;                                // ���� �������� ���������
    public int damage;                                      // ����
    public float cooldown = 3f;                             // ����������� �����
    public float timeToActive;                              // ����� ������� ���������
    bool isSpikeActive;                                     // ������� ������ ��� ���
    float lastAttack;                                       // ����� ���������� ����� (��� ����������� �����)                                                               


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (isSpikeWork && Time.time - lastAttack > cooldown)
        {
            animator.SetTrigger("SpikeHit");
            animator.SetBool("Disactive", true);
            DamageAll();
            lastAttack = Time.time;                         // ����������� ����� �����
        }
    }

    public void SpikeActiveEvent()                          // ����� ��������� �����
    {
        if (isSpikeWork)
            return;
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
        Collider2D[] collidersHits = Physics2D.OverlapBoxAll(transform.position, new Vector2 (0.5f, 0.5f), 0f);     // ������� ������� � ������� ������� � ��������
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
