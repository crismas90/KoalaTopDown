using UnityEngine;
using UnityEngine.AI;


public class Player : Fighter
{
    // Ссылки
    [HideInInspector] public Rigidbody2D rb2D;
    Animator animator;
    NavMeshAgent agent;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    // Передвижение
    [HideInInspector] public Vector2 moveDirection;     // вектор для перемещения (направление)
    Vector2 movementVector;                             // вектор перещение (добавляем скорость)
    public float moveSpeed = 5f;                        // скорость передвижения    
    //Vector3 mousePosition;                              // вектор положения мыши

    // Таймер
    float timerForColor;
    bool red;


    //---------------------------------------------------------------------------------------------------------------------------------------------------------\\


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        // Перемещение и направление
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");        
        moveDirection = new Vector2(moveX, moveY).normalized;                       // скорость нормализированная
        //UpdateMotor(moveDirection);
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);        // положение мыши

        // Анимации 
        animator.SetFloat("Speed", rb2D.velocity.magnitude);

        // Таймер для отображения урона
        if (timerForColor > 0 )
            timerForColor -= Time.deltaTime;
        if (red && timerForColor <= 0)
            ColorWhite();
    }


    private void FixedUpdate()
    {
        // Скорость
        //rb2D.AddForce(moveDirection * moveSpeed);
        //rb2D.MovePosition(rb2D.position + moveDirection * moveSpeed * Time.deltaTime);                  // скорость полная  
        rb2D.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);       // скорость полная        
    }


    /*    void UpdateMotor(Vector2 input)
        {
            movementVector = new Vector2(input.x * agent.speed, input.y * agent.speed);                      // создаем вектор куда нужно переместится        
            agent.Move(movementVector * Time.deltaTime);                                                        // перемещаем с учётом дельтаТайм
            Debug.Log(movementVector);
        }*/



    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        animator.SetTrigger("TakeHit");
        ColorRed(0.2f);
    }

    // Смена цветов при уроне
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
}