using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    Player player;                  // ссылка на игрока
    public Text hp;                 // кол-во хп
    public Text key;                // кол-во ключей
    public Text battery;            // кол-во батарей

    void Start()
    {
        player = GameManager.instance.player;
    }
   
    void Update()                                                   // (потом изменить вывод сообщенией)
    {
        // HP
        hp.text = player.currentHealth.ToString("0");

        // Ключи
        key.text = GameManager.instance.keys.ToString("0");

        // Батареи
        battery.text = GameManager.instance.battery.ToString("0");
    }
}
