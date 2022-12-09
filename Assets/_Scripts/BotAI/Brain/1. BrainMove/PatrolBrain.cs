using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brains/PatrolBrain")]
public class PatrolBrain : Brain
{
    public float lastChange;                            // время последнего удара (для перезарядки удара)
    public float cooldownChange;                        // перезардяка поиска цели
    public float distancePatrol;                        // дистанция для патрулирования
    public float maxDistancePatrol;                     // максимальная дистанция от стартовой позиции

    private void Awake()
    {
        lastChange = 0;
    }

    public override void Think(EnemyThinker thinker)
    {        
        if (Time.time - lastChange > cooldownChange)        // если кд готово
        {
            lastChange = Time.time;
            
            Vector3 destination = new(thinker.botAI.transform.position.x + Random.Range(-distancePatrol, distancePatrol), 
                thinker.botAI.transform.position.y + Random.Range(-distancePatrol, distancePatrol), thinker.botAI.transform.position.z);    // выбираем случайную позицию
            
            thinker.botAI.SetDestination(destination);                      // идём в случайную позицию            
        }

        float distanceFromStart = Vector3.Distance(thinker.botAI.startPosition, thinker.botAI.transform.position);  // считаем дистанцию от стартовой позиции до следующей позиции
        
        if (distanceFromStart > maxDistancePatrol)
            thinker.botAI.SetDestination(thinker.botAI.startPosition);      // возвращаемся в стартовую позицию        
    }
}
