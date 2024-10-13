using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBulletBehavior : AbilityBehavior
{
    public ShootBulletBehavior(GameObject playerObject) : base(playerObject)
    {
        _cooldown = 0.2f;
        _animationFilePath = "Assets/Prefab/Bullet.prefab";
        LoadImages();
    }

    public override void Activate()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.z * -1));

        Vector2 directionToMouse = (mousePosition - _playerObject.transform.position).normalized;

        Vector2 spawnLocation =  directionToMouse * 2 + (Vector2)_playerObject.transform.position;

        GameObject bullet = GameObject.Instantiate(_animationPrefab, spawnLocation, Quaternion.identity);
        bullet.layer = LayerMask.NameToLayer("Sprites");

        bullet.transform.Rotate(0, 0, (Mathf.Rad2Deg*Mathf.Atan2(directionToMouse.y, directionToMouse.x)-90f));

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = directionToMouse * 20.0f;
        bulletRb.velocity += _playerObject.GetComponent<Rigidbody2D>().velocity;
    }
}
