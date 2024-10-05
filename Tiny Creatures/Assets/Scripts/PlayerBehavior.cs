using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    [SerializeField]
    private float _playerSpeed = 5.0f;

    public float _boostMultiplier = 2.5f;

    [SerializeField]
    private int _playerHealth = 100;
    private bool _iFrameActive = false;

    public bool _boostActive = false;
    private float _iFrameTime;

    public float _boostTime;

    public float _boostTimeStart = 0;

    [SerializeField]
    private float _iFrameLength = 500f; //ms

    public float _boostLength = 50f; //ms
    public float _boostCoolDown = 3000f; //ms

    // Start is called before the first frame update
    void Start()
    {
        // Starting position
        transform.position = new Vector3(0, 0, 0);

        // Set starting health
        _playerHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");

        if (!_boostActive && Input.GetAxis("Jump") > 0 && Time.time > _boostTimeStart)
        {
            ActivateBoost();
        }

        float currentBoost = Mathf.Max(1, Convert.ToInt32(_boostActive) * _boostMultiplier);
        transform.position += (xDir * Vector3.right + yDir * Vector3.up) * _playerSpeed * currentBoost * Time.deltaTime;

        if (_iFrameActive && _iFrameTime <= Time.time)
        {
            _iFrameActive = false;
        }

        if (_boostActive && _boostTime <= Time.time)
        {
            _boostActive = false;
            _boostTimeStart = Time.time + _boostCoolDown / 1000;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            Damage(enemy.GetCollisionDamage());
        }
    }

    void Damage(int damage)
    {
        if (_playerHealth > 0 && !_iFrameActive)
        {
            _playerHealth = Mathf.Max(0, _playerHealth - damage);
            _iFrameActive = true;
            _iFrameTime = Time.time + _iFrameLength/1000;
        }

        //if (_playerHealth == 0) 
        //{
        //    Destroy(this);
        //}
    }

    void ActivateBoost()
    {
        if (!_boostActive)
        {
            _boostActive = true;
            _boostTime = Time.time + _boostLength/1000;
        }
    }

    public int GetHealth()
    {
        return _playerHealth;
    }
}
