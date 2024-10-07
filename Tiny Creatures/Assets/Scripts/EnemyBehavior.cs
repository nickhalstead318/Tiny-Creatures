using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyBehavior : MonoBehaviour
{
    private GameObject _playerObject;

    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private int _collisionDamage = 5;

    protected int health;
    protected int totalXP;
    private GameObject _experienceGem;
    private GameManagerBehavior _gameManager;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _playerObject = GameObject.FindGameObjectWithTag("Player");
        _experienceGem = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawnerBehavior>().experienceGem;
        _gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerBehavior>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!_gameManager.IsGamePaused())
        {
            Move();
        }
    }

    protected virtual void Move()
    {
        if (_playerObject != null)
        {
            Vector3 diffVector =  _playerObject.transform.position - transform.position;
            transform.position += (diffVector.normalized) * Time.deltaTime * _speed;
        }
    }

    public int GetCollisionDamage()
    {
        return _collisionDamage;
    }

    public void Damage(int damage)
    {
        health = Mathf.Max(0, health - damage);
        if (health == 0)
        {
            for(int i = 0; i < totalXP; i++)
            {
                Instantiate(_experienceGem, new Vector3(Random.Range(-0.5f,0.5f), Random.Range(-0.5f, 0.5f), 0) + transform.position, Quaternion.identity);
            }
            Destroy(transform.gameObject);
        }
    }
}
