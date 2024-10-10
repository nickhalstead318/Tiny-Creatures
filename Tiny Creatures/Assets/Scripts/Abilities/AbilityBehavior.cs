using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

abstract public class AbilityBehavior
{
    protected GameObject _playerObject;
    protected float _cooldown;
    protected float _lastActivation = -1000f;

    protected string _animationFilePath;
    protected GameObject _animationPrefab;

    protected string _abilityFrameFilePath;
    protected GameObject _abilityFramePrefab;

    public AbilityBehavior(GameObject playerObject)
    {
        _playerObject = playerObject;
    }

    public virtual void Activate()
    {
        
    }

    public virtual void Activate(GameObject abilityObject)
    {
         
    }

    public float GetCooldown()
    {
        return _cooldown;
    }

    protected void LoadImages()
    {
        if(_animationFilePath != null)
        {
            Addressables.LoadAssetAsync<GameObject>(_animationFilePath).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _animationPrefab = handle.Result; // Store the animation GameObject
                }
            };
        }
        if (_abilityFrameFilePath != null)
        {
            Addressables.LoadAssetAsync<GameObject>(_abilityFrameFilePath).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _abilityFramePrefab = handle.Result; // Store the animation GameObject
                }
            };
        }
    }
}
