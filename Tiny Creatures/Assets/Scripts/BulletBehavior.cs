using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{

    [SerializeField]
    private float _speed = 15.0f;

    private Vector3 _direction;
    private Vector3 _fireVelocity;
    private float _lifespan = 1.5f;
    private float _timeToDestroy;


    // Start is called before the first frame update
    void Start()
    {
        _timeToDestroy = Time.time + _lifespan;
    }

    public void SetDir(Vector3 dir, Vector3 fireVelocity)
    {
        _direction = dir;
        _fireVelocity = fireVelocity;
        Debug.Log("Fire Velocity: " + fireVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += (_speed * _direction + _fireVelocity) * Time.deltaTime;
        if(Time.time >= _timeToDestroy)
        {
            Destroy(transform.gameObject);
        }
    }
}
