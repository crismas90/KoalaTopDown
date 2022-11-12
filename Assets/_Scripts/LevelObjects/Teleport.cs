using UnityEngine;

public class Teleport : MonoBehaviour
{
    Animator animator;
    public GameObject exitTeleport;         // второй телепорт
    public float timeToTeleport;            // время через которое происходит телепортация    
    bool isInRange;                         // в ренже мы или нет
    bool startLoadTeleport;                 // телепортация начата
    float timerCount;                       // таймер
    GameObject goToTeleport;                // объект, который нужно телепортировать

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (isInRange && !startLoadTeleport && Input.GetKeyDown(GameManager.instance.keyToUse))
        {            
            startLoadTeleport = true;
            timerCount = timeToTeleport;            // если зашли в телепорт присваиваем время таймеру
            animator.SetBool("Teleporting", true);
        }
        if (startLoadTeleport)
        {
            Timer();                                // отсчитываем таймер
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = true;
            goToTeleport = player.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = false;
            goToTeleport = null;            
        }
    }

    void Timer()
    {
        if (timerCount > 0)                             // пока таймер больше 
            timerCount -= Time.deltaTime;
        if (timerCount <= 0)                            // если таймер закончился и игрок в ренже
            TeleportObject(goToTeleport);               // телепортируем
    }

    public void TeleportObject(GameObject go)
    {
        if (isInRange)
            go.transform.position = exitTeleport.transform.position;
        animator.SetBool("Teleporting", false);
        startLoadTeleport = false;
    }
}
