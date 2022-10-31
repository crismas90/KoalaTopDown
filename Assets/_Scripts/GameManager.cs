using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // инстанс

    [Header("—сылки")]
    public Player player;                       // ссылка на игрока

    private void Awake()
    {
        instance = this;
    }
}
