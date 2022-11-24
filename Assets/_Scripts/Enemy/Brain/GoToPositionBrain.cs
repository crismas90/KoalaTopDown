using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Brains/GoToPosition")]
public class GoToPositionBrain : Brain
{
    public Vector3 destination;

    public override void Think(EnemyThinker thinker)
    {
        thinker.botAI.SetDestination(destination);
        thinker.botAI.target = null;
        thinker.botAI.chasing = false;
    }
}
