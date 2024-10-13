using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PlayerAbilityBehavior : MonoBehaviour
{
    private GameManagerBehavior _gameManager;
    private PlayerBehavior _playerBehavior;
    private GameObject _playerObject;

    // Prefabs
    GameObject _bulletPrefab;

    private Dictionary<Ability,AbilityBehavior> _currentAbilities;
    private Dictionary<Ability, float> _currentCooldowns;

    // Start is called before the first frame update
    void Start()
    {
        // Reference to other scripts
        _gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerBehavior>();
        _playerBehavior = transform.GetComponent<PlayerBehavior>();
        _playerObject = transform.gameObject;

        // Set up starting abilities
        _currentAbilities = new Dictionary<Ability, AbilityBehavior>
        {
            { Ability.Ability1, new ShootBulletBehavior(_playerObject) },
            { Ability.Dash, new DashBehavior(_playerObject) },
        };
        _currentCooldowns = new Dictionary<Ability, float>
        {
            { Ability.Ability1, 0 },
            { Ability.Dash, 0 },
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameManager.IsGamePaused())
        {
            TickDownCooldowns();

            // Ability 1
            float ability1 = Input.GetAxis("Fire1");

            if (ability1 > 0 && CanActivate(Ability.Ability1))
            {
                AbilityBehavior abilityBehavior = _currentAbilities[Ability.Ability1];
                abilityBehavior.Activate();
                _currentCooldowns[Ability.Ability1] = abilityBehavior.GetCooldown();
            }

            // Dash
            float dash = Input.GetAxis("Jump");

            if (dash > 0 && CanActivate(Ability.Dash))
            {
                AbilityBehavior abilityBehavior = _currentAbilities[Ability.Dash];
                abilityBehavior.Activate();
                _currentCooldowns[Ability.Dash] = abilityBehavior.GetCooldown();
            }
        }
    }

    private void TickDownCooldowns()
    {
        foreach (Ability ability in _currentCooldowns.Keys.ToList())
        {
            _currentCooldowns[ability] = Mathf.Max(0, _currentCooldowns[ability]-Time.deltaTime);
            _gameManager.CooldownUpdate(ability, _currentCooldowns[ability], _currentAbilities[ability].GetCooldown());
        }
    }

    bool CanActivate(Ability ability)
    {
        if (_currentCooldowns.ContainsKey(ability) && _currentCooldowns[ability] == 0)
        {
            return true;
        }
        return false;
    }

    public enum Ability
    {
        Ability1,
        Dash
    }
}
