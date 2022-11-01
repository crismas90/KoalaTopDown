using UnityEngine;
using UnityEngine.AI;


public class Player : Fighter
{
    // Ссылки
    [HideInInspector] public Rigidbody2D rb2D;
    Animator animator;
    NavMeshAgent agent;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    [HideInInspector] public Vector3 moveDirection;     // вектор для перемещения
    //Vector3 mousePosition;                              // вектор положения мыши
    public float moveSpeed = 5f;                        // скорость передвижения    


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
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);        // положение мыши

        // Анимации 
        animator.SetFloat("Speed", rb2D.velocity.magnitude);
    }


    private void FixedUpdate()
    {
        // Скорость
        rb2D.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);       // скорость полная        
    }
}