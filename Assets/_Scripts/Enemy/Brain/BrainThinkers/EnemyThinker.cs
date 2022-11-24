using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThinker : MonoBehaviour
{
    public Brain[] brains;
    [HideInInspector] public Enemy enemy;
    bool findTarget;

    private void Awake()
    {        
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        brains[0].ThinkStart();
    }

    private void Update()
    {
        if (!enemy.target)
        {
            brains[0].Think(this);
        }

        if (enemy.target && !findTarget)
        {
            brains[1].Think(this);
            findTarget = true;
        }
    }
}
