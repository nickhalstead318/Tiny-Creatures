using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPGemBehavior : MonoBehaviour
{
    private Transform player;          // Reference to the player's transform
    [SerializeField]
    private float moveSpeed = 2f;      // Initial movement speed of the gem
    [SerializeField]
    private float acceleration = 0.5f; // Acceleration factor
    [SerializeField]
    private float delay = 10f;         // Time to wait before moving

    private bool shouldMove = false;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player in the scene by type. This assumes the player has a script attached to it, such as 'PlayerController'.
        player = FindObjectOfType<PlayerBehavior>().transform;
        if (player == null)
        {
            Debug.LogError("Player not found in the scene!");
            return;
        }

        // Start the coroutine to handle the delay before movement
        StartCoroutine(WaitAndMove());
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            MoveTowardsPlayer();
        }
    }

    IEnumerator WaitAndMove()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // After waiting, allow the gem to move towards the player
        shouldMove = true;
    }

    void MoveTowardsPlayer()
    {
        // If the player exists
        if (player != null)
        {
            // Calculate the direction towards the player
            Vector3 direction = (player.position - transform.position).normalized;

            // Move the gem towards the player, slowly accelerating
            moveSpeed += acceleration * Time.deltaTime; // Accelerate over time
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}
