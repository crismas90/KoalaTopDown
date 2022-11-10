using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    Player player;                  // ссылка на игрока
    public Text hp;                 // кол-во хп    
    public Text key;                // кол-во ключей (текст)

    void Start()
    {
        player = GameManager.instance.player;
    }
   
    void Update()
    {
        // HP
        hp.text = player.currentHealth.ToString("0");

        // Ключи
        key.text = GameManager.instance.keys.ToString("0");
    }
}
