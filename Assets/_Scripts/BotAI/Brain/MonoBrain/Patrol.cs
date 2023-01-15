using UnityEngine;

public class Patrol : MonoBehaviour
{
    EnemyThinker enemyThinker;

    public float lastChange;                            // ����� ���������� ����� (��� ����������� �����)
    public float cooldownChange;                        // ����������� ������ ����
    public float distancePatrol;                        // ��������� ��� ��������������
    public float maxDistancePatrol;                     // ������������ ��������� �� ��������� �������


    private void Awake()
    {
        enemyThinker = GetComponent<EnemyThinker>();
    }

    void FixedUpdate()
    {
        if (!enemyThinker.patrolingRandomPosition)
            return;

        if (Time.time - lastChange > Random.Range(cooldownChange, cooldownChange + 2f))        // ���� �� ������
        {
            lastChange = Time.time;

            Vector3 destination = new(enemyThinker.botAI.transform.position.x + Random.Range(-distancePatrol, distancePatrol),
                enemyThinker.botAI.transform.position.y + Random.Range(-distancePatrol, distancePatrol), enemyThinker.botAI.transform.position.z);    // �������� ��������� �������

            enemyThinker.botAI.SetDestination(destination);                      // ��� � ��������� �������            
        }

        float distanceFromStart = Vector3.Distance(enemyThinker.botAI.startPosition, enemyThinker.botAI.transform.position);  // ������� ��������� �� ��������� ������� �� ��������� �������

        if (distanceFromStart > maxDistancePatrol)
            enemyThinker.botAI.SetDestination(enemyThinker.botAI.startPosition);      // ������������ � ��������� �������        
    }
}

