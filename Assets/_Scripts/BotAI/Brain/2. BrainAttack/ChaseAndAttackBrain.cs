using UnityEngine;


[CreateAssetMenu(menuName = "Brains/ChaseAndAttackBrain")]
public class ChaseAndAttackBrain : Brain
{
    public override void Think(EnemyThinker thinker)
    {
        float distance = Vector3.Distance(thinker.botAI.transform.position, thinker.target.transform.position);       // ������� ��������� �� ����        
        if (thinker.botAI.targetVisible && distance < thinker.botAI.distanceToAttack)
        {         
            if (!thinker.botAI.readyToAttack)
            {
                thinker.botAI.agent.ResetPath();                                                              // ���������� ����            
                thinker.botAI.readyToAttack = true;                                                           // ����� ��������
            }
        }
        else 
        {
            thinker.botAI.agent.SetDestination(thinker.target.transform.position);                              // ������������ � ����
            if (thinker.botAI.readyToAttack)
                thinker.botAI.readyToAttack = false;                                                            // �� ����� ��������                
        }

        //Debug.Log(range); 
        
    }
}