using UnityEngine;
using UnityEngine.Events;

public class TeleportLevel : MonoBehaviour
{

    public GameObject exitTeleport;         // ������ ��������      
    public bool actived;                    // �������� �����������
    bool isInRange;                         // � ����� �� ��� ���    
    GameObject goToTeleport;                // ������, ������� ����� ���������������

    void ActivateTeleport()
    {
        actived = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = true;
            goToTeleport = player.gameObject;
            if (actived)
                TeleportObject(goToTeleport);                   // �������������
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            isInRange = false;
            goToTeleport = null;
        }
    }

    public void TeleportObject(GameObject go)
    {
        if (isInRange)
            go.transform.position = exitTeleport.transform.position;        
    }

/*    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }*/
}
