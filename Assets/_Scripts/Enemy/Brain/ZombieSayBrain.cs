using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brains/ZombieSayBrain")]
public class ZombieSayBrain : Brain
{
    public string textToSay;

    public override void Think(EnemyThinker thinker)
    {
        thinker.botAI.SayText(textToSay);        
    }
}
