using UnityEngine;
using UnityEngine.AI;

public class BotAI : Fighter
{
    // Ссылки
    EnemyThinker enemyThinker;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;    
    BotAIHitBoxPivot pivot;
    [HideInInspector] public BotAIMeleeWeaponHolder botAIMeleeWeaponHolder;
    BotAIHitbox hitBox;

    // Тип бота
    public bool isNeutral;                                  // не будет никого атаковать
    public bool isFriendly;                                 // союзный бот

    // Преследование
    [HideInInspector] public LayerMask layerTarget;         // слой для поиска 
    [HideInInspector] public LayerMask layerHit;            // слой для оружия
    [HideInInspector] public bool chasing;                  // статус преследования
    Vector3 startPosition;                                  // позиция для охраны
    public float chaseLeght;                                // дальность преследования    
    public float triggerLenght;                             // дистанция тригера
    [HideInInspector] public bool targetVisible;            // видим мы цель или нет
    public bool readyToAttack;                              // можно атаковать
    public float distanceToAttackMelee;                     // дистанция для атакой мили
    public float distanceToAttackRange;                     // дистанция для атаки ренж
    [HideInInspector] public float distanceToAttack;        // дистанция, с которой можно атаковать

    public bool meleeAttackType;                            // устанавливаем тип атаки мили
    public bool rangeAttackType;                            // ... ренж
    public bool twoWeapons;
    //public bool switchMelee;

    // Для анимации
    [HideInInspector] public float aimAnglePivot;           // угол поворота хитбокспивота
    public GameObject deathEffect;                          // эффект (потом сделать его в аниматоре (или  нет))
    public float deathCameraShake;                          // мощность тряски камеры при убийстве
    [HideInInspector] public bool flipLeft;                  // для флипа
    [HideInInspector] public bool flipRight;                 //    

    // Таймер для цветов при уроне
    float timerForColor;
    bool red;

    // Дебаг
    public bool debug;

    public override void Awake()
    {
        base.Awake();
        enemyThinker = GetComponentInChildren<EnemyThinker>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pivot = GetComponentInChildren<BotAIHitBoxPivot>();
        botAIMeleeWeaponHolder = GetComponentInChildren<BotAIMeleeWeaponHolder>();
        hitBox = GetComponentInChildren<BotAIHitbox>();

        layerTarget = LayerMask.GetMask("Player", "NPC");
        layerHit = LayerMask.GetMask("Player", "NPC", "ObjectsDestroyble", "Default");
        if (isFriendly)
        {
            layerTarget = LayerMask.GetMask("Enemy");                                   // слой поиска цели
            gameObject.layer = LayerMask.NameToLayer("NPC");                            // слой самого бота
            layerHit = LayerMask.GetMask("Enemy", "ObjectsDestroyble", "Default");      // слой для оружия
        }
    }

    void Start()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        agent.updateRotation = false;                       // для навмеш2д
        agent.updateUpAxis = false;                         //
        agent.ResetPath();                                  // сбрасываем путь, потому что он при старте есть

        if (meleeAttackType)
            SwitchAttackType(1);
        if (rangeAttackType)
            SwitchAttackType(2);
    }

    private void Update()
    {
        // Поворот хитбокса
        if (enemyThinker.target && chasing && targetVisible)
        {
            Vector3 aimDirection = enemyThinker.target.transform.position - pivot.transform.position;               // угол между положением мыши и pivot оружия          
            aimAnglePivot = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                            // находим угол в градусах             
            Quaternion qua1 = Quaternion.Euler(0, 0, aimAnglePivot);                                                // создаем этот угол в Quaternion
            pivot.transform.rotation = Quaternion.Lerp(pivot.transform.rotation, qua1, Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
        }

        // поворот спрайта (Flip)       
        if (enemyThinker.target && targetVisible)                           // (потом chasing заменить на target и ещё это дублируется в хитбокспивот)
        {
            if (Mathf.Abs(aimAnglePivot) > 90 && !flipLeft)
            {
                FaceTargetLeft();
                pivot.Flip();
            }
            if (Mathf.Abs(aimAnglePivot) <= 90 && !flipRight)
            {
                FaceTargetRight();
                pivot.Flip();
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

    public void SwitchAttackType(int type)
    {        
        if (type == 1)          // мили
        {
            meleeAttackType = true;
            rangeAttackType = false;
            distanceToAttack = distanceToAttackMelee;
        }
        if (type == 2)          // ренж
        {
            meleeAttackType = false;
            rangeAttackType = true;
            distanceToAttack = distanceToAttackRange;
        }
        
/*        else
        {
            if (type == 1)          // мили
            {
                botAIMeleeWeaponHolder.SelectCurrentWeapon(0);
                distanceToAttack = distanceToAttackMelee;
            }
            if (type == 2)          // ренж
            {
                botAIMeleeWeaponHolder.SelectCurrentWeapon(1);
                distanceToAttack = distanceToAttackRange;
            }
            switchMelee = true;
        }*/
    }


    public void NavMeshRayCast(GameObject target)
    {
        NavMeshHit hit;
        if (!agent.Raycast(target.transform.position, out hit))
        {
            //Debug.Log("Visible");            
            targetVisible = true;                                           // цели видима из нашей позиции
        }
        else
        {
            // тут добавить проверку какой объект попал под рейкаст (стена или снаряд например или враг)
            targetVisible = false;
        }
    }


/*    public void ChaseAndAttack(GameObject target)
    {        
        float distance = Vector3.Distance(transform.position, target.transform.position);       // считаем дистанцию до цели
        if (distance < distanceToAttack && targetVisible)                                       // если дошли до цели и видим её
        {
            if (!readyToAttack)
            {
                agent.ResetPath();                                                              // сбрасываем путь            
                readyToAttack = true;                                                           // готов стрелять
                //Debug.Log("Ready Attack");
            }
        }
        else 
        {
            agent.SetDestination(target.transform.position);                                    // перемещаемся к цели
            if (readyToAttack)
                readyToAttack = false;                                                          // не готов стрелять                
        }
    }*/

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void SayText(string text)
    {
        ChatBubble.Clear(gameObject);
        ChatBubble.Create(transform, new Vector3(0.2f, 0.2f), text);
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





    public void ForceBackFire(Vector3 forceDirection, float forceBack)
    {
        Vector2 vec2 = (transform.position - forceDirection).normalized;        // направление отдачи нормализированное
        rb2D.AddForce(vec2 * forceBack, ForceMode2D.Impulse);                   // толкаем импульсом
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