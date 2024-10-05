using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AbilityBehavior
{
    protected GameObject _playerObject;
    protected float _cooldown;
    protected float _maxCooldown;
    protected float _lastActivation = -1000f;

    public AbilityBehavior(GameObject playerObject)
    {
        _playerObject = playerObject;
    }

    protected virtual void Activate()
    {
        
    }

    protected virtual void Activate(GameObject abilityObject)
    {
         
    }

    public bool TryToActivate(GameObject abilityObject)
    {
        if (CanActivate())
        {
            if(abilityObject == null)
            {
                Activate();
            }
            else
            {
                Activate(abilityObject);
            }
            _lastActivation = Time.time;
            return true;
        }

        return false;
    }

    public virtual bool CanActivate()
    {
        if(Time.time >= _lastActivation + _cooldown)
        {
            return true;
        }
        return false;
    }

    protected void SetInitialCooldown(float cooldown)
    {
        _maxCooldown = cooldown;
        _cooldown = _maxCooldown;
    }
}
