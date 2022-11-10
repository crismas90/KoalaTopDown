using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventInteract : MonoBehaviour
{
    [Header("Параметры")]
    public bool withArrow;
    public KeyCode key;
    bool isInRange;
    public SpriteRenderer spriteButtomUse;
    public UnityEvent interactAction;

    private void Start()
    {
        spriteButtomUse.enabled = false;
    }

    public void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(key))
            {
                interactAction.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = true;
            if (withArrow)
                spriteButtomUse.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = false;
            if (withArrow)
                spriteButtomUse.enabled = false;
        }
    }
}
