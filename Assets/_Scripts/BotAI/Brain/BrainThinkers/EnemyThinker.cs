using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public BotAI botAI;
    public Brain[] brains;

    [HideInInspector] public GameObject target;     // цель
    bool isFindTarget;
    float distanceToTarget;                         // дистанция до цели


    public bool isFriendly;
    public GameObject[] patrolPoints;
    [HideInInspector] public int i = 0;    

    // Поиск цели
    //public float targetFindRadius = 5f;                 // радиус поиска цели                                                               
    float lastTargetFind;                               // время последнего удара (для перезарядки удара)
    float cooldownFind = 0.5f;                          // перезардяка атаки
    LayerMask layerTarget;                              // слой для поиска цели

    private void Awake()
    {        
        botAI = GetComponent<BotAI>();
        layerTarget = LayerMask.GetMask("Player", "NPC");
    }

    private void Start()
    {
        
    }

    /// <summary>
    /// brains[0].Think(this); - поведение когда нет цели (стоять на месте, патруль и т.д.)
    /// brains[1].Think(this); - преследуем и атакуем   
    /// </summary>



    private void FixedUpdate()
    {
        if (botAI.isNeutral)                        // если бот нейтрален
            return;

        // Сбрасываем всякие штуки, если цели нет
        if (isFindTarget && !target)
        {
            isFindTarget = false;
            botAI.chasing = false;                  // преследование отключено
            //botAI.target = null;
            botAI.targetVisible = false;
            botAI.readyToAttack = false;
            botAI.agent.ResetPath();
        }


        // Логика для поиска цели и патрулирования 
        if (!isFindTarget)                              // если нет цели 
        {            
            brains[0].Think(this);                      // патрулирование                     
            FindTarget();                               // поиск цели  
        }

        // Если нашли цель делаем рейкасты и меряем дистанцию
        if (target)                                     // если нашли цель
        {
            botAI.NavMeshRayCast(target);               // делаем рейкаст
            distanceToTarget = Vector3.Distance(target.transform.position, botAI.transform.position);   // считаем дистанцию 
        }


        // Логика если нашли цель и она видима        
        if (distanceToTarget < botAI.triggerLenght && botAI.targetVisible)       // если дистанция до игрока < тригер дистанции
        {
            if (!botAI.chasing)
            {
                botAI.chasing = true;                   // преследование включено
                isFindTarget = true;
            }
        }   

        if (botAI.chasing && target)
        {
            brains[1].Think(this);                      // преследуем и аттакуем           
        }

        //Debug.Log(target);
        //Debug.Log(isFindTarget);
        //Debug.Log(botAI.chasing);
        //Debug.Log(botAI.readyToAttack);


        if (isFriendly)
        {
            MakeFriendly();
            isFriendly = false;
        }
    }


    void FindTarget()
    {
        if (!target && Time.time - lastTargetFind > cooldownFind)                       // если готовы атаковать и кд готово
        {
            lastTargetFind = Time.time;                                                 // присваиваем время атаки
            Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, botAI.triggerLenght, layerTarget);    // создаем круг в позиции объекта с радиусом 
            foreach (Collider2D enObjectBox in collidersHitbox)
            {
                if (enObjectBox == null)
                {
                    continue;
                }

                if (enObjectBox.gameObject.TryGetComponent(out Fighter fighter))        // ищем скрипт файтер
                {
                    NavMeshHit hit;
                    if (!botAI.agent.Raycast(fighter.transform.position, out hit))      // делаем рейкаст
                    {
                        target = fighter.gameObject;
                        //botAI.target = target;
                    }
                }
                collidersHitbox = null;                                                 // сбрасываем все найденные объекты (на самом деле непонятно как это работает)
            }
        }
    }

    void MakeFriendly()
    {
        layerTarget = LayerMask.GetMask("Enemy");               // слой поиска цели
        botAI.gameObject.layer = LayerMask.NameToLayer("NPC");  // слой самого бота
        botAI.layerHit = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");    // слой для оружия
        foreach (GameObject weaponGO in botAI.botAIWeaponHolder.weapons)                // слой для каждого оружия у бота
        {
            var weapon = weaponGO.GetComponent<BotAIWeapon>();
            weapon.layerHit = botAI.layerHit;
        }        
        //botAI.hitBox.layer = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");
        //botAI.spriteRenderer.color = Color.yellow;        
    }
}
