using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Brains/GoToPosition")]
public class GoToPositionBrain : Brain
{
    public bool go;    
    

    public override void Think(EnemyThinker thinker)
    {
        if (thinker.nextPosition && thinker.positionsPoints.Length > 0)
        {
            thinker.i++;
            thinker.nextPosition = false;
            if (thinker.i > thinker.positionsPoints.Length - 1)
                thinker.i = 0;
        }

        if (thinker.letsGo)
        {
            thinker.botAI.SetDestination(thinker.positionsPoints[thinker.i].position);
        }
    }
}
