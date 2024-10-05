using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    [SerializeField]
    private float _playerSpeed = 5.0f;

    [SerializeField]
    private int _playerHealth = 100;
    private bool _iFrameActive = false;
    private float _iFrameTime = 0;

    [SerializeField]
    private float _iFrameLength = 500;

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
        
        transform.position += (xDir * Vector3.right + yDir * Vector3.up) * _playerSpeed * Time.deltaTime;

        if (_iFrameActive && _iFrameTime <= Time.time)
        {
            _iFrameActive = false;
        }
    }

    private void OnTriggerStay(Collider other)
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

    public int GetHealth()
    {
        return _playerHealth;
    }
}
