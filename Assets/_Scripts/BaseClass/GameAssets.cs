using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;         // инстанс

    public Transform chatBubblePrefab;
    public GameObject floatingText;

    private void Awake()
    {
        instance = this;
    }
}
