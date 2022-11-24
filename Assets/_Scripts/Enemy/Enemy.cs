using UnityEngine;
using UnityEngine.AI;

public class Enemy : Fighter
{
    // Ссылки
    NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    SpriteRenderer spriteRenderer;
    EnemyHitBoxPivot pivot;
    EnemyHitbox hitBox;

    // Преследование
    public bool isNeutral;                                  // не будет никого атаковать
    [HideInInspector] public GameObject target;             // цель
    [HideInInspector] public bool chasing;                  // статус преследования
    public float triggerLenght;                             // дистанция тригера
    public float distanceToAttack;                          // дистанция, с которой можно атаковать (0.8 для мелкого)
    [HideInInspector] public bool targetVisible;            // видим мы цель или нет
    [HideInInspector] public bool readyToAttack;            // можно атаковать

    
    //public float attackSpeed = 1;                           // скорость атаки

    // Для анимации
    public GameObject deathEffect;                          // эффект (потом сделать его в аниматоре (или  нет))
    public float deathCameraShake;                          // мощность тряски камеры при убийстве
    bool flipLeft;                                          // для флипа
    bool flipRight;                                         //    

    // Таймер для цветов при уроне
    float timerForColor;
    bool red;

    // Дебаг
    public bool debug;

    public override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pivot = GetComponentInChildren<EnemyHitBoxPivot>();
        hitBox = GetComponentInChildren<EnemyHitbox>();
    }

    void Start()
    {
        //target = GameManager.instance.player.gameObject;    // пока что цель только игрок

        agent.updateRotation = false;                       // для навмеш2д
        agent.updateUpAxis = false;                         //
    }

    private void Update()
    {
        // поворот спрайта (Flip)       
        if (chasing && targetVisible)                           // (потом chasing заменить на target и ещё это дублируется в хитбокспивот)
        {
            if (Mathf.Abs(pivot.aimAngle) > 90 && !flipLeft)
            {
                FaceTargetLeft();
            }
            if (Mathf.Abs(pivot.aimAngle) <= 90 && !flipRight)
            {
                FaceTargetRight();
            }
        }
        else
        {
            if (agent.velocity.x < -0.2 && !flipLeft)
            {
                FaceTargetLeft();
            }
            if (agent.velocity.x > 0.2 && !flipRight)
            {
                FaceTargetRight();
            }
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);

        // Выбор цвета при получении урона и его сброс
        SetColorTimer();

        // Дебаг
        if (debug)
            Debug.Log(targetVisible);
    }

    void FixedUpdate()
    {
        if (!target)
        {            
            return;
        }
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
        if (Vector3.Distance(target.transform.position, transform.position) < triggerLenght && targetVisible)       // если дистанция до игрока < тригер дистанции
        {
            chasing = true;                                                 // преследование включено 
        }

        if (chasing)                                                        // если преследуем
        {
            Chase(target);
        }

        if (debug)
        {
            //Debug.Log(chasing);
        }
    }

    public void Chase(GameObject target)
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

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }


    // Фукция для ивента анимации (потом как-нибудь сделать по нормальному)
    public void AttacHitBox()               
    {
        hitBox.Attack();
    }   

    public void EffectRangeAttackHitBox()
    {
        hitBox.EffectRangeAttack();
    }



    public void SayText(string text)
    {
        ChatBubble.Clear(gameObject);
        ChatBubble.Create(transform, new Vector3(0.2f, 0.2f), text);
    }

    public void ForceBackFire(Vector3 forceDirection, float forceBack)
    {
        Vector2 vec2 = (transform.position - forceDirection).normalized;        // направление отдачи нормализированное
        rb2D.AddForce(vec2 * forceBack, ForceMode2D.Impulse);                   // толкаем импульсом
    }


    public override void TakeDamage(int dmg)
    {
        if (currentHealth == maxHealth)             // если получили урон, но жизни были полные
            chasing = true;
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
        CMCameraShake.Instance.ShakeCamera(deathCameraShake, 0.2f);                             // тряска камеры
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);  // создаем эффект убийства
        //effect.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Destroy(effect, 1);                                                                     // уничтожаем эффект через .. сек
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToAttack);
    }
}