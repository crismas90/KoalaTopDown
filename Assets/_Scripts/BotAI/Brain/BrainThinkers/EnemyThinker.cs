using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class EnemyThinker : MonoBehaviour
{
    [HideInInspector] public BotAI botAI;
    public Brain[] brains;

    [HideInInspector] public GameObject target;     // ����
    bool isFindTarget;                              // ����� ����
    float distanceToTarget;                         // ��������� �� ����

    public Transform[] positionsPoints;
    [HideInInspector] public int i = 0;    
    public bool nextPosition;
    public bool letsGo;

    // ����� ����
    //public float targetFindRadius = 5f;                 // ������ ������ ����                                                               
    float lastTargetFind;                               // ����� ���������� ����� (��� ����������� �����)
    float cooldownFind = 0.5f;                          // ����������� �����

    bool type_1;        // ��� ������ ����
    bool type_2;        // ��� ������ ����


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
    /// brains[0].Think(this); - ��������� ����� ��� ���� (������ �� �����, ������� � �.�.)
    /// brains[1].Think(this); - ���������� � �������   
    /// </summary>



    private void FixedUpdate()
    {
        // ���� ��� ���������
        if (botAI.isNeutral)                        
            return;

        // ���������� ������ �����, ���� ���� ���
        if (isFindTarget && !target)
        {
            isFindTarget = false;
            botAI.chasing = false;                      // ������������� ���������            
            botAI.targetVisible = false;
            botAI.readyToAttack = false;
            botAI.agent.ResetPath();
        }

        // ������ ��� ������ ���� � �������������� 
        if (!isFindTarget)                              // ���� ��� ���� 
        {            
            brains[0].Think(this);                      // ��������������                     
            FindTarget();                               // ����� ����
        }

        // ���� ����� ���� ������ �������� � ������ ���������
        if (target)                                     // ���� ����� ����
        {
            botAI.NavMeshRayCast(target);               // ������ �������
            distanceToTarget = Vector3.Distance(target.transform.position, botAI.transform.position);   // ������� ��������� 
        }

        // ������ ���� ����� ���� � ��� ������        
        if (botAI.targetVisible)       // ���� ��������� �� ������ < ������ ���������       (distanceToTarget < botAI.triggerLenght &&)
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
                botAI.chasing = true;                   // ������������� ��������
                isFindTarget = true;
            }
        }   

        if (botAI.chasing && target)
        {
            brains[1].Think(this);                      // ���������� � ��������           
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
        if (!target && Time.time - lastTargetFind > cooldownFind)                       // ���� ������ ��������� � �� ������
        {
            lastTargetFind = Time.time;                                                 // ����������� ����� �����
            Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, botAI.triggerLenght, botAI.layerTarget);    // ������� ���� � ������� ������� � �������� 
            foreach (Collider2D enObjectBox in collidersHitbox)
            {
                if (enObjectBox == null)
                {
                    continue;
                }

                if (enObjectBox.gameObject.TryGetComponent(out Fighter fighter))        // ���� ������ ������
                {
                    botAI.NavMeshRayCast(fighter.gameObject);
                    if (botAI.targetVisible)
                    {
                        target = fighter.gameObject;
                    }

/*                    NavMeshHit hit;
                    if (!botAI.agent.Raycast(fighter.transform.position, out hit))      // ������ �������
                    {
                                                
                    }*/
                }
                collidersHitbox = null;                                                 // ���������� ��� ��������� ������� (�� ����� ���� ��������� ��� ��� ��������)
            }
        }
    }

    public void GoNextPosition()
    {
        nextPosition = true;
    }

    public void LetsGo()
    {
        letsGo = true;
    }

    void MakeFriendly()
    {
        
        botAI.gameObject.layer = LayerMask.NameToLayer("NPC");  // ���� ������ ����
        botAI.layerHit = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");    // ���� ��� ������
        
/*        foreach (GameObject weaponGO in botAI.botAIWeaponHolder.weapons)                // ���� ��� ������� ������ � ����
        {
            var weapon = weaponGO.GetComponent<BotAIWeapon>();
            weapon.layerHit = botAI.layerHit;
        }  */      
       
    }
}
