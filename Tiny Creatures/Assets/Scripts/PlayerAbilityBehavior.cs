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

        //Reference to Prefabs
        //Addressables.LoadAssetAsync<GameObject>("Assets/Prefab/Bullet.prefab");

        _currentAbilities = new Dictionary<Ability, AbilityBehavior>
        {
            { Ability.Ability1, new ShootBulletBehavior(_playerObject) }
        };
        _currentCooldowns = new Dictionary<Ability, float>
        {
            { Ability.Ability1, 0 }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameManager.IsGamePaused())
        {
            TickDownCooldowns();

            // Ability 1
            float att1 = Input.GetAxis("Fire1");

            if (att1 > 0 && CanActivate(Ability.Ability1))
            {
                AbilityBehavior abilityBehavior = _currentAbilities[Ability.Ability1];
                abilityBehavior.Activate();
                _currentCooldowns[Ability.Ability1] = abilityBehavior.GetCooldown();
            }
        }
    }

    private void TickDownCooldowns()
    {
        foreach (Ability ability in _currentCooldowns.Keys.ToList())
        {
            _currentCooldowns[ability] = Mathf.Max(0, _currentCooldowns[ability]-Time.deltaTime);
            _gameManager.CooldownUpdate(Ability.Ability1, _currentCooldowns[ability], _currentAbilities[Ability.Ability1].GetCooldown());
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
        Ability1
    }
}
