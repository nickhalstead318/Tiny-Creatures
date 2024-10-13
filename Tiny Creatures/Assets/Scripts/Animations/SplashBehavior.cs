using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashBehavior : MonoBehaviour
{
    [SerializeField]
    private int _damage = 2;

    private GameManagerBehavior _gameManager;
    private GameObject _playerObject;
    private float _speed = 10.0f;
    private List<Collider2D> _colliders;

    [SerializeField]
    private float _pushbackForce = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerBehavior>();
        _colliders = new List<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameManager.IsGamePaused())
        {
            transform.localScale += new Vector3(1, 1, 0) * Time.deltaTime * _speed;
            if (transform.localScale.x > 10)
            {
                Destroy(transform.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !_colliders.Contains(collision))
        {
            _colliders.Add(collision);
            EnemyBehavior enemy = collision.GetComponent<EnemyBehavior>();

            Vector2 pushVector = (enemy.transform.position - transform.position);

            enemy.Halt(0.5f);
            enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(pushVector.normalized * _pushbackForce, ForceMode2D.Impulse);
            enemy.Damage(_damage);
        }
    }
}
