using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventInteract : MonoBehaviour
{
    [Header("���������")]
    public bool withArrow;                      // �� �������� ��� ���   
    public bool withButton;                      // �� �������� ��� ���   
    public UnityEvent interactAction;           // �����
    bool isInRange;                             // � ����� ��� ���
    GameObject arrow;                           // �������

    private void Start()
    {
        arrow = transform.Find("QuestArrow").gameObject;
        arrow.SetActive(false);
    }

    public void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(GameManager.instance.keyToUse) || !withButton)
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
                arrow.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = false;
            if (withArrow)
                arrow.SetActive(false);
        }
    }
}
