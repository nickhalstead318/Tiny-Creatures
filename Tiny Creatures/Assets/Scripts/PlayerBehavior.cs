using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    [SerializeField]
    private float _playerSpeed = 5.0f; // Speed of player

    [SerializeField]
    private float _dashMultiplier = 2.5f; // Speed multiplier

    [SerializeField]
    private int _playerHealth = 100;

    [SerializeField]
    private AudioSource damageSound; // When player gets hit

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

    private GameObject _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Starting position
        transform.position = new Vector3(0, 0, 0);

        // Add reference to Game Manager
        _gameManager = GameObject.FindGameObjectWithTag("Manager");
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
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

        float currentBoost = Mathf.Max(1, Convert.ToInt32(_dashActive) * _dashMultiplier);
        transform.position += (xDir * Vector3.right + yDir * Vector3.up) * _playerSpeed * currentBoost * Time.deltaTime;

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

    // Ow! That actually hurt
    void Damage(int damage)
    {

        if (_playerHealth > 0 && !_iFrameActive)
        {
            ActivateIFrames(_iFrameLength);

            // Play damage sound
            if (damageSound != null)
            {
                damageSound.Play();
            }

            _playerHealth = Mathf.Max(0, _playerHealth - damage);
        }
        
        if (_playerHealth == 0) {
            Destroy(transform.gameObject);
            _gameManager.GetComponent<GameManagerBehavior>().OnPlayerDeath();
        }
    }

    // This activates the player's dash
    void ActivateDash()
    {
        ActivateIFrames(_dashLength);

        if (!_dashActive)
        {
            _dashActive = true;
            _dashTime = Time.time + _dashLength;
        }
    }

    // Activate iframes for X seconds
    void ActivateIFrames(float frameLength)
    {
        _iFrameActive = true;
        _iFrameTime = Time.time + frameLength;
    }

    public int GetHealth()
    {
        return _playerHealth;
    }
}
