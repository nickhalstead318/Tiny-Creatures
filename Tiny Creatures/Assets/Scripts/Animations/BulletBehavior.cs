using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{

    [SerializeField]
    private int _damage = 2;

    private Vector2 _fireVelocity;
    private float _lifespan = 1.5f;
    private float _timeLeftToDestroy;
    private Rigidbody2D _rigidBody;

    private GameManagerBehavior _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _timeLeftToDestroy = _lifespan;

        _gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerBehavior>();
        _rigidBody = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_fireVelocity.sqrMagnitude == 0 && _rigidBody.velocity.sqrMagnitude != 0)
        {
            _fireVelocity = _rigidBody.velocity;
        }

        if (!_gameManager.IsGamePaused())
        {
            if(_rigidBody.velocity.sqrMagnitude == 0)
            {
                _rigidBody.velocity = _fireVelocity;
            }

            if (_timeLeftToDestroy <= 0)
            {
                Destroy(transform.gameObject);
            }
            else
            {
                _timeLeftToDestroy -= Time.deltaTime;
            }
        }
        else
        {
            _rigidBody.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(transform.gameObject);
            EnemyBehavior enemy = collision.GetComponent<EnemyBehavior>();
            enemy.Damage(_damage);
        }
    }
}
