using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBulletBehavior : AbilityBehavior
{
    public ShootBulletBehavior(GameObject playerObject) : base(playerObject)
    {
        SetInitialCooldown(0.1f);
    }

    protected override void Activate(GameObject abilityObject)
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));

        mouseWorldPosition.z = 0f;
        
        //Vector3 mouseWorldPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 directionToMouse = mouseWorldPosition - _playerObject.transform.position;

        directionToMouse.Normalize();
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");
        Vector3 firingVelocity = (xDir * Vector3.right + yDir*Vector3.up).normalized * _playerObject.GetComponent<PlayerBehavior>().CalcCurrentSpeed();

        Debug.Log("Mouse: " + mouseWorldPosition.x + ", " + mouseWorldPosition.y);
        Debug.Log("Player: " + _playerObject.transform.position.x + ", " + _playerObject.transform.position.y);
        
        Vector3 spawnLocation = directionToMouse + _playerObject.transform.position;
        
        GameObject bullet = GameObject.Instantiate(abilityObject, spawnLocation, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = (15f * directionToMouse + firingVelocity);
        //bullet.GetComponent<BulletBehavior>().SetDir(directionToMouse,firingVelocity);
    }
}
