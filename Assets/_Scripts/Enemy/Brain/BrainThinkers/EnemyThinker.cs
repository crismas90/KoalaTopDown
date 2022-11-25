using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public Enemy botAI;
    public Brain[] brains;

    [HideInInspector] public GameObject target;     // цель
    float distanceToTarget;                         // дистанция до цели


    public bool isFriendly;
    public GameObject[] patrolPoints;
    int i = 0;
    public bool go;

    private void Awake()
    {        
        botAI = GetComponent<Enemy>();
    }

    private void FixedUpdate()
    {
        if (botAI.isNeutral)                    // если бот нейтрален
            return;

        // тут логика для поиска цели и патрулирования




        

        if (!target && !botAI.agent.hasPath && go)                            // если нет цели и нет пути
        {
            //brains[0].Think(this);              // поиск цели

                      
            botAI.SetDestination(patrolPoints[i].transform.position);
            i++;
            Debug.Log("ПОзиция !");
            return;
        }

        if (target)                             // если нашли цель
        {
            botAI.NavMeshRayCast(target);       // делаем рейкаст
            distanceToTarget = Vector3.Distance(target.transform.position, botAI.transform.position);   // считаем дистанцию
        }      

        // тут логика если нашли цель и она видима

        // Преследование 
        if (!botAI.chasing && distanceToTarget < botAI.triggerLenght && botAI.targetVisible)       // если дистанция до игрока < тригер дистанции
        {
            botAI.chasing = true;                   // преследование включено                                              
        }   
        if (botAI.chasing)
        {
            botAI.Chase(target);                    // преследуем
        }

        if (isFriendly)
        {
            MakeFriendly();
            isFriendly = false;
        }
    }

    void MakeFriendly()
    {
        botAI.hitBox.layer = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        botAI.gameObject.layer = LayerMask.NameToLayer("NPC");
        botAI.spriteRenderer.color = Color.yellow;
        brains[1].Think(this);              // поиск цели
    }
}
