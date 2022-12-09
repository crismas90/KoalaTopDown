using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public float triggerRadius;
    public LayerMask layer;

    public void TriggerEnemy()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, triggerRadius, layer);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<BotAI>(out BotAI enemy))            // (потом переделать на скрипт Enemy)
            {
                enemy.triggerLenght = 25;
            }
            collidersHits = null;
        }
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
