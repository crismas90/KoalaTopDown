using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // инстанс

    [Header("Ссылки")]
    public Player player;                       // ссылка на игрока

    [Header("Предметы")]
    public int keys;                            // ключи

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChatBubble.Create(player.transform, new Vector3(0f, 0f), "Hello !!! Hello !!! Hello !!! Hello !!! Hello !!! Hello !!! Hello !!!");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ChatBubble.Create(player.transform, new Vector3(0f, 0f), "Hello!!!Hello!!!Hello!!!Hello!!Hello!!!Hello!!!Hello !!!");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            ChatBubble.Create(player.transform, new Vector3(0f, 0f), "hi");
        }
    
    }

    public void TakeKey(bool findKey)
    {
        if (findKey)
            keys++;
        else if (!findKey && keys > 0)
            keys--;
    }
}
