using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    // Player Constants
    [SerializeField]
    private float _playerSpeed = 5.0f; // Speed of player
    [SerializeField]
    private float _dashMultiplier = 2.5f; // Speed multiplier
    [SerializeField]
    private int _maxHealth = 100;
    [SerializeField]
    private int _playerHealth;
    [SerializeField]
    private int _currentXp = 0;
    private int _xpToNextLevel = 15;
    private int _playerLevel = 1;

    // Player Damage
    public AudioClip damageSound; // When player gets hit
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Color damageColor = Color.red;  // Color to flash when hit
    [SerializeField]
    private Color dashColor = new Color(20, 20, 70); // Color to flash when dash
    [SerializeField]
    private float flashDuration = 0.1f;     // How long the flash lasts
    private Color originalColor; // Revert to this after taking damage

    // Experience
    [SerializeField]
    public AudioClip xpSound; // When player gets hit

    // Status Effects
    // Is player currently invincible / dashing
    private bool _iFrameActive = false;
    private bool _dashActive = false;

    // Time in (s)econds iframe/dash ends from game start
    private float _iFrameTime;
    private float _dashTime;

    private float _dashTimeStart = 0; // Time in (s)econds dash can be activated again from game start

    [SerializeField]
    private float _iFrameLength = 0.5f; // Length of iframes in (s)econds
    public float _dashLength = 0.05f; // Length of dash in (s)econds
    public float _dashCoolDown = 3.0f; // Length of dash cooldown in (s)econds

    private Vector3 _currentVelocity;

    // Player Attacks
    [SerializeField]
    private GameObject _attack1;

    // Misc
    private GameManagerBehavior _gameManager;

    // Debug
    private AbilityBehavior _fireGun;

    // Start is called before the first frame update
    void Start()
    {
        // Starting position
        transform.position = new Vector3(0, 0, 0);

        // Starting Health
        _playerHealth = _maxHealth;

        // Original color
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // Add reference to Game Manager
        _gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerBehavior>();

        _fireGun = new ShootBulletBehavior(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameManager.IsGamePaused())
        {
            CalculateMovement();

            CalculateAttacks();
        }
    }

    // Calculates everything related to movement
    private void CalculateMovement()
    {
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");

        // If not dashing, player tries to dash, and not on cooldown
        if (!_dashActive && Input.GetAxis("Jump") > 0 && Time.time > _dashTimeStart)
        {
            ActivateDash();
        }

        float currentSpeed = CalcCurrentSpeed();
        _currentVelocity = (xDir * Vector3.right + yDir * Vector3.up) * currentSpeed * Time.deltaTime;
        Vector3 newPos = transform.position + _currentVelocity;
        transform.position = Mathf.Clamp(newPos.x, -34, 24) * Vector3.right + Mathf.Clamp(newPos.y, -24, 34) * Vector3.up;

        //transform.position += (xDir * Vector3.right + yDir * Vector3.up) * currentSpeed * Time.deltaTime;


        if (_iFrameActive && _iFrameTime <= Time.time)
        {
            _iFrameActive = false;
        }

        if (_dashActive && _dashTime <= Time.time)
        {
            _dashActive = false;
            _dashTimeStart = Time.time + _dashCoolDown;
        }
    }

    // What happens when things go bump in the night?
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            Damage(enemy.GetCollisionDamage());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Experience Gem")
        {
            Destroy(other.transform.gameObject);
            GainExperience(1);
        }
    }

    // Ow! That actually hurt
    void Damage(int damage)
    {

        if (_playerHealth > 0 && !_iFrameActive)
        {
            ActivateIFrames(_iFrameLength);

            StartCoroutine(FlashDamageCoroutine());

            // Play damage sound
            if (damageSound != null)
            {
                transform.GetComponent<AudioSource>().PlayOneShot(damageSound);
            }

            UpdateHealth(-1 * damage);
        }
        
        if (_playerHealth == 0) {
            Destroy(transform.gameObject);
            _gameManager.OnPlayerDeath();
        }
    }

    void UpdateHealth(int change)
    {
        _playerHealth = Mathf.Clamp(_playerHealth + change, 0, _maxHealth);
        _gameManager.UpdatePlayerHealth(_playerHealth, _maxHealth);
    }

    void GainExperience(int xpGained)
    {
        // Play XP sound
        if (xpSound != null)
        {
            transform.GetComponent<AudioSource>().PlayOneShot(xpSound);
        }

        bool increaseDiff = false;
        _currentXp += xpGained;
        if(_currentXp >= _xpToNextLevel)
        {
            _currentXp -= _xpToNextLevel;
            _playerLevel += 1;
            increaseDiff = true;
            _xpToNextLevel = _playerLevel*10 + 5;
        }
        _gameManager.UpdatePlayerExperience(_currentXp, _xpToNextLevel, _playerLevel, increaseDiff);
    }

    private IEnumerator FlashDamageCoroutine()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    // This activates the player's dash
    void ActivateDash()
    {
        ActivateIFrames(_dashLength);

        _dashActive = true;
        _dashTime = Time.time + _dashLength;

        StartCoroutine(PostDash());
    }

    IEnumerator PostDash()
    {
        // Turn blue during dash
        spriteRenderer.color = dashColor;

        // Length of dash
        yield return new WaitForSeconds(_dashLength);

        // Reset color
        spriteRenderer.color = originalColor;

    }

    // Activate iframes for X seconds
    void ActivateIFrames(float frameLength)
    {
        _iFrameActive = true;
        _iFrameTime = Time.time + frameLength;
    }

    void CalculateAttacks()
    {
        if (_currentVelocity.sqrMagnitude > 0)
        {
            return;
        }

        float att1 = Input.GetAxis("Fire1");
        
        if (att1 > 0 && _attack1 != null)
        {
            FireAttack(Attacks.Attack1);
        }
    }

    enum Attacks
    {
        Attack1
    }

    void FireAttack(Attacks attack)
    {
        switch(attack)
        {
            case Attacks.Attack1:
                _fireGun.TryToActivate(_attack1);
                break;
            default: break;
        }
    }
    
    public float CalcCurrentSpeed()
    {
        float currentBoost = Mathf.Max(1, Convert.ToInt32(_dashActive) * _dashMultiplier);
        return currentBoost * _playerSpeed;
    }

    // Getters
    public int GetHealth()
    {
        return _playerHealth;
    }

    public float GetSpeed()
    {
        return _playerSpeed;
    }
}
