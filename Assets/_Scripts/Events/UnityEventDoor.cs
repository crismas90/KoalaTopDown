using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventDoor : MonoBehaviour
{
    public UnityEvent doorEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.TryGetComponent<Player>(out Player player) && GameManager.instance.keys > 0)
        {            
            doorEvent.Invoke();                 
        }
    }

    public void OpenDoor()
    {

    }
}
