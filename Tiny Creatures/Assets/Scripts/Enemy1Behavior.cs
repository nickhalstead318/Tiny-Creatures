using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Behavior : EnemyBehavior
{
    // Start is called before the first frame update
    protected override void Start()
    {
        health = 10;
        totalXP = 1;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
