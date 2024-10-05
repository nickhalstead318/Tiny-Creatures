using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{

    [SerializeField]
    private float _playerSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Starting position
        transform.position = new Vector3(0, 0, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        float xDir = Input.GetAxis("Horizontal");

        float yDir = Input.GetAxis("Vertical");
        

        transform.position += (xDir * Vector3.right + yDir * Vector3.up) * _playerSpeed * Time.deltaTime;
    }
}
