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
    float cooldownFind = 0.5f;                          // ����������� ������ ����
    float cooldownChangeTarget = 2f;                    // ����������� ����� ����

    bool type_1;        // ��� ������ ����
    bool type_2;        // ��� ������ ����

    public string textTrigger;
    bool sayTriggerText;
    
    [Header("���������")]
    public bool patrolingRandomPosition;


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
        if (!isFindTarget)                                  // ���� ��� ���� 
        {
            if (Time.time - lastTargetFind > cooldownFind)  // ���� �� ������
            {
                FindTarget();                               // ����� ����
            }

            if (letsGo)
            {
                brains[0].Think(this);                          // �������������� 
            }
            if (!letsGo)
            {
                patrolingRandomPosition = true;                 // ��������������            
            }
        }
        else
        {
            if (Time.time - lastTargetFind > Random.Range(cooldownChangeTarget, cooldownChangeTarget + 2f))  // ������� ����, ���� ������ ����� ����
            {
                FindTarget();                               // ����� ����
            }
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
                patrolingRandomPosition = false;        // �������������� 
                botAI.chasing = true;                   // ������������� ��������
                isFindTarget = true;
            }
        }   

        if (botAI.chasing && target)
        {
            brains[1].Think(this);                      // ���������� � ��������           
        }



        if (sayTriggerText && !isFindTarget)
        {
            botAI.SayText(textTrigger);
            sayTriggerText = false;
        }



        if(debug)
        {
            Debug.Log(target);
            //Debug.Log(isFindTarget);
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
        lastTargetFind = Time.time;                                                 // ����������� ����� �����
        Collider2D[] collidersHitbox = Physics2D.OverlapCircleAll(transform.position, botAI.triggerLenght, botAI.layerTarget);    // ������� ���� � ������� ������� � �������� 
        List<GameObject> targets = new List<GameObject>(); 
        foreach (Collider2D enObjectBox in collidersHitbox)
        {
            if (enObjectBox == null)
            {
                continue;
            }

            if (enObjectBox.gameObject.TryGetComponent(out Fighter fighter))        // ���� ������ ������
            {
                botAI.NavMeshRayCast(fighter.gameObject);
                float distance = Vector3.Distance(fighter.transform.position, botAI.transform.position);   // ������� ��������� 
                if (!target)
                {
                    if (botAI.targetVisible)
                    {
                        targets.Add(fighter.gameObject);
                    }
                }
                else
                {
                    if (botAI.targetVisible && distance < 3)
                    {
                        targets.Add(fighter.gameObject);                            
                    }
                }       
            }
            collidersHitbox = null;                         // ���������� ��� ��������� ������� (�� ����� ���� ��������� ��� ��� ��������)
        }
        if (targets.Count > 0)
            target = targets[Random.Range(0, targets.Count)];            
        
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

    public void StayGo()
    {
        letsGo = !letsGo;
        if (letsGo)
        {
            botAI.SayText("� �� �����");
        }
        if (!letsGo)
        {
            botAI.SayText("��� �����");
        }
    }


    public void TriggerSayText(string text)
    {
        textTrigger = text;
        sayTriggerText = true;
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
