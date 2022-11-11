using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public void PickUpKey()
    {
        GameManager.instance.TakeKey(true);
        Destroy(gameObject);
    }
}
