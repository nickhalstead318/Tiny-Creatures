using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{

    [SerializeField]
    private float _speed = 15.0f;

    [SerializeField]
    private int _damage = 5;

    private Vector3 _direction;
    private Vector3 _fireVelocity;
    private float _lifespan = 1.5f;
    private float _timeToDestroy;

    private GameManagerBehavior _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _timeToDestroy = Time.time + _lifespan;

        _gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerBehavior>();
    }

    public void SetDir(Vector3 dir, Vector3 fireVelocity)
    {
        _direction = dir;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameManager.IsGamePaused())
        {
            transform.position += (_speed * _direction) * Time.deltaTime;
            if (Time.time >= _timeToDestroy)
            {
                Destroy(transform.gameObject);
            }
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
