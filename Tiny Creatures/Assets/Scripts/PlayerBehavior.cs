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
    private Vector2 _playerMovement; // Speed of player
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
    public bool dashActive = false;

    // Time in (s)econds iframe/dash ends from game start
    private float _iFrameTime;

    [SerializeField]
    private float _iFrameLength = 0.5f; // Length of iframes in (s)econds

    // Dash
    [SerializeField]
    private float _dashMultiplier = 2.5f;
    [SerializeField]
    private float _dashLength = 0.3f; // Length of dash in (s)econds

    // Player Attacks
    [SerializeField]
    private GameObject _attack1;

    // Misc
    private GameManagerBehavior _gameManager;
    private PlayerAbilityBehavior _playerAbilities;

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

        // Reference to other scripts
        _gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerBehavior>();
        _playerAbilities = transform.GetComponent<PlayerAbilityBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameManager.IsGamePaused())
        {
            CalculateMovement();

            //CalculateAttacks();
        }
    }

    // Calculates everything related to movement
    private void CalculateMovement()
    {
        _playerMovement.x = Input.GetAxisRaw("Horizontal");
        _playerMovement.y = Input.GetAxisRaw("Vertical");

        if (_iFrameActive && _iFrameTime <= Time.time)
        {
            _iFrameActive = false;
        }
    }

    private void FixedUpdate()
    {
        if (!_gameManager.IsGamePaused() && !dashActive)
        {
            transform.GetComponent<Rigidbody2D>().velocity = _playerMovement * _playerSpeed;
        }
        else if(_gameManager.IsGamePaused())
        {
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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

    public void Dash()
    {
        StartCoroutine(DashRoutine(transform.gameObject));
    }

    private IEnumerator DashRoutine(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Vector2 originalVelocity = rb.velocity;
        dashActive = true;
        ActivateIFrames(_dashLength);
        spriteRenderer.color = dashColor;

        rb.velocity *= _dashMultiplier;

        yield return new WaitForSeconds(_dashLength);

        rb.velocity = originalVelocity; // Reset to the original speed
        spriteRenderer.color = originalColor;
        dashActive = false;
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

    // Activate iframes for X seconds
    void ActivateIFrames(float frameLength)
    {
        _iFrameActive = true;
        _iFrameTime = Time.time + frameLength;
    }

    enum Attacks
    {
        Attack1
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
