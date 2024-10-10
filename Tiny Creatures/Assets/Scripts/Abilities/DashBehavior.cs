using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBehavior : AbilityBehavior
{
    public DashBehavior(GameObject playerObject) : base(playerObject)
    {
        _cooldown = 3f;
        LoadImages();
    }

    public override void Activate()
    {
        _playerObject.GetComponent<PlayerBehavior>().Dash();
    }
}
