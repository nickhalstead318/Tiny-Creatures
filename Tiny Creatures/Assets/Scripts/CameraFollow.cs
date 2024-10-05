using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // The player's position
    public Vector3 offset = new Vector3(0, 0, -10); // Camera's offset from the player
    public float smoothSpeed = 0.25f;  // How quickly the camera catches up to the player (lower values = slower)

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.position + offset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Target position based on player position and offset
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;

            // Smoothly interpolate between current camera position and the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
    }
}
