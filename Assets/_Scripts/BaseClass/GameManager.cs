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

    public void TakeKey(bool findKey)
    {
        if (findKey)
            keys++;
        else if (!findKey && keys > 0)
            keys--;
    }
}
