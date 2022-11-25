using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brains/PatrolBrain")]
public class PatrolBrain : Brain
{
    public override void Think(EnemyThinker thinker)
    {
        thinker.botAI.SetDestination(thinker.patrolPoints[0].transform.position);
    }
}
