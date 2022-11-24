using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Brains/Zombie Brain")]
public class ZombieSayBrain : Brain
{
    public string textToSay;
    public override void ThinkStart()
    {

    }
    public override void Think(EnemyThinker thinker)
    {
        var enemy = thinker.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.SayText(textToSay);
        }
    }
}
