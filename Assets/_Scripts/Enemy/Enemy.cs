using UnityEngine;
using UnityEngine.AI;

public class Enemy : Fighter
{
    // Ссылки
    NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    SpriteRenderer spriteRenderer;

    // Преследование
    public bool isNeutral;                                   // не будет никого атаковать
    [HideInInspector] public GameObject target;             // цель
    [HideInInspector] public bool chasing;                  // статус преследования
    public float triggerLenght;                             // дистанция тригера
    public float distanceToAttack;                          // дистанция, с которой можно атаковать
    [HideInInspector] public bool targetVisible;            // видим мы цель или нет
    [HideInInspector] public bool readyToAttack;            // можно атаковать

    // Атака
    [HideInInspector] public float lastAttack;              // время последнего удара (для перезарядки удара)
    public float cooldown = 0.5f;                           // перезардяка атаки
    public int attackDamage;                                // урон
    public float pushForce;                                 // сила толчка
    
    //public float attackSpeed = 1;                           // скорость атаки

    // Для анимации
    public GameObject deathEffect;                          // эффект (потом сделать его в аниматоре (или  нет))
    bool flipLeft;                                          // для флипа
    bool flipRight;                                         //    

    // Таймер для цветов при уроне
    float timerForColor;
    bool red;

    public override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        target = GameManager.instance.player.gameObject;    // пока что цель только игрок

        agent.updateRotation = false;                       // для навмеш2д
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

        // Выбор цвета при получении урона и его сброс
        SetColorTimer();
    }

    void FixedUpdate()
    {
        if (!target)
            return;
        if (isNeutral)
            return;

        NavMeshHit hit;
        if (!agent.Raycast(target.transform.position, out hit))
        {
            //Debug.Log("Visible");            
            targetVisible = true;                                           // Target is "visible" from our position.
        }
        else
        {
            // тут добавить проверку какой объект попал под рейкаст (стена или снаряд например или враг)
            targetVisible = false;
        }

        // Преследование
        if (Vector3.Distance(target.transform.position, transform.position) < triggerLenght && targetVisible || currentHealth != maxHealth)       // если дистанция до игрока < тригер дистанции
        {
            chasing = true;                                                 // преследование включено 
        }

        if (chasing)                                                        // если преследуем
        {
            //agent.SetDestination(target.transform.position);                    // перемещаемся к цели


            float distance = Vector3.Distance(transform.position, target.transform.position);       // считаем дистанцию до цели
            if (distance < distanceToAttack && targetVisible)                                       // если дошли до цели и видим её
            {
                agent.ResetPath();                                                                  // сбрасываем путь            
                readyToAttack = true;                                                               // готов стрелять
                //Debug.Log("Ready Attack");
            }
            else
            {
                agent.SetDestination(target.transform.position);                                    // перемещаемся к цели
                readyToAttack = false;                                                              // не готов стрелять                
            }
        }
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        //animator.SetTrigger("TakeHit");
        ColorRed(0.05f);
    }

    // Смена цветов при уроне
    void SetColorTimer()
    {
        if (timerForColor > 0)                  // таймер для отображения урона
            timerForColor -= Time.deltaTime;
        if (red && timerForColor <= 0)
            ColorWhite();
    }
    void ColorRed(float time)
    {
        timerForColor = time;
        spriteRenderer.color = Color.red;
        red = true;
    }
    void ColorWhite()
    {
        spriteRenderer.color = Color.white;
        red = false;
    }

    // Поворот спрайта
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
        CMCameraShake.Instance.ShakeCamera(3, 0.2f);                                            // тряска камеры
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);  // создаем эффект
        Destroy(effect, 1);                                                                     // уничтожаем эффект через .. сек
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToAttack);
    }

    public void SayText(string text)
    {
        ChatBubble.Clear(gameObject);
        ChatBubble.Create(transform, new Vector3(0.2f, 0.2f), text);
    }
}
