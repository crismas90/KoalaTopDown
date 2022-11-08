using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventKey : MonoBehaviour
{
    public UnityEvent keyEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision!");
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            //Debug.Log("Event!");
            keyEvent.Invoke();
        }
    }
}
