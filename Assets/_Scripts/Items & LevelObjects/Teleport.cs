using UnityEngine;
using UnityEngine.Events;

public class Teleport : MonoBehaviour
{
    Player player;
    Animator animator;
    public GameObject exitTeleport;         // второй телепорт
    public float timeToTeleport;            // врем€ через которое происходит телепортаци€    
    public bool actived;                    // телепорт активирован
    public int batteryToActivate;           // сколько батарей нужно, чтобы активировать телепорт
    //public UnityEvent interactAction;       // ивент

    bool isInRange;                         // в ренже мы или нет
    bool startedLoadTeleport;               // телепортаци€ начата
    float timerCount;                       // таймер
    GameObject goToTeleport;                // объект, который нужно телепортировать

    private void Start()
    {
        player = GameManager.instance.player;
        animator = GetComponent<Animator>();
        if (actived)
        {
            animator.SetBool("Activated", true);
        }
    }

    public void ActivateTeleportWithDelay()
    {
        if (GameManager.instance.battery >= batteryToActivate && !actived)
        {
            Invoke("ActivateTeleport", 1);
            animator.SetBool("Activated", true);
            GameManager.instance.battery -= batteryToActivate;                      // забираем батареи
            player.SayText("“еперь телепорт активирован");
        }
        else if (GameManager.instance.battery < batteryToActivate && !actived)
        {
            player.SayText("” мен€ недостаточно батарей дл€ активации телепорта");
        }
    }
    void ActivateTeleport()
    {
        actived = true;
        
    }

    void Update()
    {
        if (actived && isInRange && !startedLoadTeleport && Input.GetKeyDown(GameManager.instance.keyToUse))
        {            
            startedLoadTeleport = true;
            timerCount = timeToTeleport;            // если зашли в телепорт присваиваем врем€ таймеру
            animator.SetBool("Teleporting", true);
        }
        if (startedLoadTeleport)
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
        if (timerCount <= 0)                            // если таймер закончилс€ и игрок в ренже
            TeleportObject(goToTeleport);               // телепортируем
    }

    public void TeleportObject(GameObject go)
    {
        if (isInRange)
            go.transform.position = exitTeleport.transform.position;
        animator.SetBool("Teleporting", false);
        startedLoadTeleport = false;
    }
}
