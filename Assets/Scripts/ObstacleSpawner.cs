using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // The obstacle prefab to spawn
    public float spawnInterval = 1f;  // The interval between obstacle spawns

    public Vector3 moveDirection1 = Vector3.forward; // First obstacle move direction
    public Vector3 moveDirection2 = Vector3.back;    // Second obstacle move direction
    public float moveSpeed = 15f; // Speed at which the obstacles move

    private Coroutine spawnCoroutine; // Reference to the spawn coroutine

    // Public method to start spawning obstacles
    public void StartSpawning()
    {
        if (spawnCoroutine == null) // Ensure only one coroutine runs at a time
        {
            Debug.Log("Starting obstacle spawning.");
            spawnCoroutine = StartCoroutine(SpawnObstacles());
        }
    }

    // Coroutine to spawn obstacles at regular intervals
    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            Debug.Log("Spawning an obstacle.");

            // Random delay between spawns
            float randomDelay = Random.Range(0.5f * spawnInterval, 1f * spawnInterval);
            yield return new WaitForSeconds(randomDelay);

            // Define spawn position
            Vector3 spawnPosition = new Vector3(
                Random.Range(1.8f, 2.5f), // Adjust the X range as needed
                Random.Range(-10f, -9f),  // Adjust the Y range as needed
                23f                       // Fixed Z position
            );

            // Instantiate the first obstacle and set its movement direction
            GameObject obstacle1 = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
            obstacle1.AddComponent<ObstacleMovement>().Initialize(moveDirection1, moveSpeed);

            // Instantiate the second obstacle and set its movement direction
            GameObject obstacle2 = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
            obstacle2.AddComponent<ObstacleMovement>().Initialize(moveDirection2, moveSpeed);
        }
    }
}
