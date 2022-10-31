using UnityEngine;
using UnityEngine.AI;

public class Enemy : Fighter
{
    NavMeshAgent agent;
    Animator animator;

    [HideInInspector]
    public GameObject target;                       // цель

    [HideInInspector]
    public bool targetVisible;                      // видим мы цель или нет

    [HideInInspector] 
    public bool readyToFire;                        // можно стрелять

    public float distanceToShoot;                   // дистанция, с которой можно стрелять
    public float faceingTargetSpeed;                // скорость поворота 


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameManager.instance.player.gameObject;        // пока что цель только игрок

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {

    }

    void FixedUpdate()
    {
/*        if (!target)
            return;

        NavMeshHit hit;
        if (!agent.Raycast(target.transform.position, out hit))
        {
            //Debug.Log("Visible");            
            targetVisible = true;                                       // Target is "visible" from our position.
        }
        else
        {
            // тут добавить проверку какой объект попал под рейкаст (стена или снаряд например или враг)
            targetVisible = false;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);       // считаем дистанцию до цели
        if (distance < distanceToShoot && targetVisible)                                        // если дошли до цели и видим её
        {
            agent.ResetPath();                                                                  // сбрасываем путь
            FaceTarget();                                                                       // разворачиваемся к ней
            readyToFire = true;                                                                 // готов стрелять
            animator.SetBool("Runing", false);                                                   // анимация ног (потом переделать)
        }
        else
        {
            agent.SetDestination(target.transform.position);                                    // перемещаемся к цели
            readyToFire = false ;                                                               // не готов стрелять
            animator.SetBool("Runing", true);                                                   // анимация ног (потом переделать)
        }*/

        agent.SetDestination(target.transform.position);                                    // перемещаемся к цели

    }       


    void FaceTarget()                                                                           // поворот к цели
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * faceingTargetSpeed);
    }

    protected override void Death()
    {
        base.Death();
        Destroy(gameObject);
    }
}
