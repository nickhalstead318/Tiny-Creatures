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

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
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
}
