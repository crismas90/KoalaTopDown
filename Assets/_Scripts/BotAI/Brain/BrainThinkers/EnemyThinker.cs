using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public BotAI botAI;
    public Brain[] brains;

    [HideInInspector] public GameObject target;     // цель
    bool isFindTarget;                              // нашли цель
    float distanceToTarget;                         // дистанция до цели

    public Transform[] positionsPoints;
    [HideInInspector] public int i = 0;    
    public bool nextPosition;
    public bool letsGo;

    // Поиск цели
    //public float targetFindRadius = 5f;                 // радиус поиска цели                                                               
    float lastTargetFind;                               // время последнего удара (для перезарядки удара)
    float cooldownFind = 0.5f;                          // перезардяка атаки

    bool type_1;        // тип оружия мили
    bool type_2;        // тип оружия ренж


    //public bool isFriendly;

    public bool debug;
    

    private void Awake()
    {        
        botAI = GetComponent<BotAI>();        
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
        // Если бот нейтрален
        if (botAI.isNeutral)                        
            return;

        // Сбрасываем всякие штуки, если цели нет
        if (isFindTarget && !target)
        {
            isFindTarget = false;
            botAI.chasing = false;                      // преследование отключено            
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
        if (botAI.targetVisible)       // если дистанция до игрока < тригер дистанции       (distanceToTarget < botAI.triggerLenght &&)
        {
            if (botAI.twoWeapons)
            {
                if (distanceToTarget < 2 && !type_1)
                {
                    botAI.SwitchAttackType(1);
                    type_1 = true;
                    type_2 = false;
                }
                if (distanceToTarget > botAI.triggerLenght - 1 && !type_2)
                {
                    botAI.SwitchAttackType(2);
                    type_1 = false;
                    type_2 = true;
                }
            }                
 

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

        if(debug)
        {
            //Debug.Log(target);
            Debug.Log(isFindTarget);
            //Debug.Log(botAI.chasing);
            //Debug.Log(botAI.readyToAttack);
        }


/*        if (isFriendly)
        {
            MakeFriendly();
            isFriendly = false;
        }*/
    }


    void FindTarget()
    {
        if (!target && Time.time - lastTargetFind > cooldownFind)                       // если готовы атаковать и кд готово
        {
            lastTargetFind = Time.time;                                                 // присваиваем время атаки
            Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, botAI.triggerLenght, botAI.layerTarget);    // создаем круг в позиции объекта с радиусом 
            foreach (Collider2D enObjectBox in collidersHitbox)
            {
                if (enObjectBox == null)
                {
                    continue;
                }

                if (enObjectBox.gameObject.TryGetComponent(out Fighter fighter))        // ищем скрипт файтер
                {
                    botAI.NavMeshRayCast(fighter.gameObject);
                    if (botAI.targetVisible)
                    {
                        target = fighter.gameObject;
                    }

/*                    NavMeshHit hit;
                    if (!botAI.agent.Raycast(fighter.transform.position, out hit))      // делаем рейкаст
                    {
                                                
                    }*/
                }
                collidersHitbox = null;                                                 // сбрасываем все найденные объекты (на самом деле непонятно как это работает)
            }
        }
    }

    public void GoToPosition(int positionNumber)
    {
        /*        if (nextPosition && positionsPoints.Length > 0)
                {

                }*/
        i = positionNumber;
    }

    public void LetsGo(int go)
    {
        if (go == 0)
            letsGo = false;
        if (go == 1)
            letsGo = true;
    }

    void MakeFriendly()
    {
        
        botAI.gameObject.layer = LayerMask.NameToLayer("NPC");  // слой самого бота
        botAI.layerHit = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");    // слой для оружия
        
/*        foreach (GameObject weaponGO in botAI.botAIWeaponHolder.weapons)                // слой для каждого оружия у бота
        {
            var weapon = weaponGO.GetComponent<BotAIWeapon>();
            weapon.layerHit = botAI.layerHit;
        }  */      
       
    }
}
