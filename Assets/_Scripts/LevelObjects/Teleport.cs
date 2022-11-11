using UnityEngine;

public class Teleport : MonoBehaviour
{
    Animator animator;
    public GameObject exitTeleport;         // второй телепорт
    public float timeToTeleport;            // время через которое происходит телепортация
    bool isInRange;                         // в ренже мы или нет
    float timerCount;                       // таймер
    GameObject goToTeleport;                // объект, который нужно телепортировать

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (isInRange)                      // если в ренже
        {
            Timer();                        // отсчитываем таймер
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = true;
            goToTeleport = player.gameObject;
            timerCount = timeToTeleport;                // если зашли в телепорт присваиваем время таймеру
            animator.SetBool("Teleporting", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = false;
            goToTeleport = null;
            animator.SetBool("Teleporting", false);
        }
    }

    void Timer()
    {
        if (timerCount > 0)                             // пока таймер больше 
            timerCount -= Time.deltaTime;
        if (timerCount <= 0)                            // если таймер закончился
            TeleportObject(goToTeleport);               // телепортируем
    }

    public void TeleportObject(GameObject go)
    {
        go.transform.position = exitTeleport.transform.position;
    }
}
