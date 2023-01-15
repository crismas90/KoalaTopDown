using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brains/TargetFollowBrain")]
public class TargetFollowBrain : Brain
{
    //public GameObject tagTarget;
    float distanceToPlayer;

    public override void Think(EnemyThinker thinker)
    {
        distanceToPlayer = Vector3.Distance(GameManager.instance.player.transform.position, thinker.botAI.transform.position);
        if (distanceToPlayer > 2)
        {
            thinker.botAI.SetDestination(GameManager.instance.player.transform.position);
        }
/*        else
        {
            thinker.botAI.agent.ResetPath();
        }*/
    }
}
