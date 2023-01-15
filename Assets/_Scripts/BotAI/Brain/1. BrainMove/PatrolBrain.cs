using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brains/PatrolBrain")]
public class PatrolBrain : Brain
{
    public float lastChange;                            // ����� ���������� ����� (��� ����������� �����)
    public float cooldownChange;                        // ����������� ������ ����
    public float distancePatrol;                        // ��������� ��� ��������������
    public float maxDistancePatrol;                     // ������������ ��������� �� ��������� �������

    private void Awake()
    {
        lastChange = 0;
    }

    public override void Think(EnemyThinker thinker)
    {        
        if (Time.time - lastChange > cooldownChange)        // ���� �� ������
        {
            lastChange = Time.time;
            
            Vector3 destination = new(thinker.botAI.transform.position.x + Random.Range(-distancePatrol, distancePatrol), 
                thinker.botAI.transform.position.y + Random.Range(-distancePatrol, distancePatrol), thinker.botAI.transform.position.z);    // �������� ��������� �������
            
            thinker.botAI.SetDestination(destination);                      // ��� � ��������� �������            
        }

        float distanceFromStart = Vector3.Distance(thinker.botAI.startPosition, thinker.botAI.transform.position);  // ������� ��������� �� ��������� ������� �� ��������� �������
        
        if (distanceFromStart > maxDistancePatrol)
            thinker.botAI.SetDestination(thinker.botAI.startPosition);      // ������������ � ��������� �������        
    }
}
