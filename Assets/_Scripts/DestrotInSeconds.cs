using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestrotInSeconds : MonoBehaviour
{
    [SerializeField] float secondsToDestroy;

    void Start()
    {
        Destroy(gameObject, secondsToDestroy);
    }
}
