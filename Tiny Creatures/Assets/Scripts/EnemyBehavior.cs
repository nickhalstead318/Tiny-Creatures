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
    protected Rigidbody2D _rigidBody;
    protected bool _canMove = true;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _playerObject = GameObject.FindGameObjectWithTag("Player");
        _experienceGem = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawnerBehavior>().experienceGem;
        _gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerBehavior>();

        _rigidBody = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        if (!_gameManager.IsGamePaused() && _canMove)
        {
            Move();
        }
        else if (_gameManager.IsGamePaused())
        {
            _rigidBody.velocity = Vector2.zero;
        }
    }

    protected virtual void Move()
    {
        if (_playerObject != null)
        {
            Vector3 diffVector =  _playerObject.transform.position - transform.position;

            _rigidBody.velocity = diffVector.normalized * _speed;
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

    public void Halt(float timeDelay)
    {
        StartCoroutine(PauseMovement(timeDelay));
    }

    protected IEnumerator PauseMovement(float timeDelay)
    {
        _canMove = false;
        yield return new WaitForSeconds(timeDelay);

        _canMove = true;
    }
}
