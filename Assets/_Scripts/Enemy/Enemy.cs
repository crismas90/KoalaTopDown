using UnityEngine;
using UnityEngine.AI;

public class Enemy : Fighter
{
    // —сылки
    NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    SpriteRenderer spriteRenderer;
    [HideInInspector] public Rigidbody2D rb2D;

    // ѕреследование
    [HideInInspector] public GameObject target;             // цель
    bool chasing;                                           // статус преследовани€
    public float triggerLenght;                             // дистанци€ тригера
    public float distanceToAttack;                          // дистанци€, с которой можно атаковать
    [HideInInspector] public bool targetVisible;            // видим мы цель или нет
    [HideInInspector] public bool readyToAttack;            // можно атаковать

    // јтака
    [HideInInspector] public float lastAttack;              // врем€ последнего удара (дл€ перезар€дки удара)
    public float cooldown = 0.5f;                           // перезард€ка атаки
    public int attackDamage;                                // урон
    public float pushForce;                                 // сила толчка
    public float attackRadius;                              // радиус атаки
    //public float attackSpeed = 1;                           // скорость атаки

    // ƒл€ анимации
    bool flipLeft;                                          // дл€ флипа
    bool flipRight;                                         //    


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();

        target = GameManager.instance.player.gameObject;    // пока что цель только игрок

        agent.updateRotation = false;                       // дл€ навмеш2д
        agent.updateUpAxis = false;                         //
    }

    private void Update()
    {
        // поворот спрайта (Flip)       (потом лучше заменить на более правильный)
        if (agent.velocity.x < -0.2 && !flipLeft)
        {
            FaceTargetLeft();
        }
        if (agent.velocity.x > 0.2 && !flipRight)
        {
            FaceTargetRight();
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void FixedUpdate()
    {
        if (!target)
            return;

        // ѕреследование
        if (Vector3.Distance(target.transform.position, transform.position) < triggerLenght)       // если дистанци€ до игрока < тригер дистанции
        {
            chasing = true;                                                 // преследование включено 
        }

        if (chasing)                                                        // если преследуем
        {
            //agent.SetDestination(target.transform.position);                    // перемещаемс€ к цели
            NavMeshHit hit;
            if (!agent.Raycast(target.transform.position, out hit))
            {
                //Debug.Log("Visible");            
                targetVisible = true;                                       // Target is "visible" from our position.
            }
            else
            {
                // тут добавить проверку какой объект попал под рейкаст (стена или снар€д например или враг)
                targetVisible = false;
            }

            float distance = Vector3.Distance(transform.position, target.transform.position);       // считаем дистанцию до цели
            if (distance < distanceToAttack && targetVisible)                                       // если дошли до цели и видим еЄ
            {
                agent.ResetPath();                                                                  // сбрасываем путь            
                readyToAttack = true;                                                               // готов стрел€ть
                //Debug.Log("Ready Attack");
            }
            else
            {
                agent.SetDestination(target.transform.position);                                    // перемещаемс€ к цели
                readyToAttack = false;                                                              // не готов стрел€ть                
            }
        }
    }


    void FaceTargetRight()                                          // поворот направо
    {
        spriteRenderer.flipX = false;
        flipLeft = false;
        flipRight = true;
    }
    void FaceTargetLeft()                                           // поворот налево
    {
        spriteRenderer.flipX = true;
        flipRight = false;
        flipLeft = true;
    }

    protected override void Death()
    {
        base.Death();
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position, attackRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
