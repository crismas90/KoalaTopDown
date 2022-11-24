using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Brains/Chase")]
public class ChaseBrain : Brain
{
    public string targetTag;
    GameObject target;
    public override void ThinkStart()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);
    }
    public override void Think(EnemyThinker thinker)
    {         
        if (target)
        {
            thinker.enemy.target = target;
        }
    }
}
