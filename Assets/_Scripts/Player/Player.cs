using UnityEngine;
using UnityEngine.AI;


public class Player : Fighter
{
    // Ссылки
    Rigidbody2D rb;
    Animator animator;
    NavMeshAgent agent;

    [HideInInspector] 
    public Vector3 moveDirection;
    Vector3 mousePosition;
    public float moveSpeed = 5f;
    //public float turnSpeed;


    //---------------------------------------------------------------------------------------------------------------------------------------------------------\\


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        // Перемещение и направление
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        moveDirection = new Vector2(moveX, moveY).normalized;                       // скорость нормализированная
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);        // положение мыши
    }


    private void FixedUpdate()
    {
        // Скорость
        rb.velocity = new Vector2(moveDirection.x * moveSpeed,  moveDirection.y * moveSpeed);                 // скорость полная        
        
        // Анимации

    }
}