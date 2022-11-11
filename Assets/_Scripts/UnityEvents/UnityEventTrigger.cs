using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventTrigger : MonoBehaviour
{
    [Header("Параметры")]
    public bool isSingleTrigger;
    public UnityEvent interactAction;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            interactAction.Invoke();
            if (isSingleTrigger)
            {
                Destroy(gameObject);
            }                
        }
    }
}
