using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{

    [SerializeField]
    private float _speed = 1.0f;

    private Vector3 _direction;



    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetDir(Vector3 dir)
    {
        _direction = dir;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _speed * _direction * Time.deltaTime;
    }
}
