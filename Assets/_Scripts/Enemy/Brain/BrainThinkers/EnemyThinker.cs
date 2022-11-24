using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThinker : MonoBehaviour
{
    public Brain[] brains;
    [HideInInspector] public Enemy botAI;
    bool findTarget;
    public bool targetPlayer;
    public bool retreat;

    private void Awake()
    {        
        botAI = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (retreat)
        {
            brains[3].Think(this);
            findTarget = false;
            return;
        }

        if (!botAI.target)
        {
            if (targetPlayer)
                brains[1].Think(this);
            if (!targetPlayer)
                brains[0].Think(this);
        }
                
        if (botAI.target && !findTarget)
        {
            brains[2].Think(this);
            findTarget = true;
        }
        Debug.Log(botAI.target);

    }
}
