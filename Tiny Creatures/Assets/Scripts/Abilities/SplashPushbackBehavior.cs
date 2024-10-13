using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashPushbackBehavior : AbilityBehavior
{
    public SplashPushbackBehavior(GameObject playerObject) : base(playerObject)
    {
        _cooldown = 6f;
        _animationFilePath = "Assets/Prefab/Splash Animation.prefab";
        LoadImages();
    }

    public override void Activate()
    {
        GameObject splash = GameObject.Instantiate(_animationPrefab, _playerObject.transform.position, Quaternion.identity);
    }
}
