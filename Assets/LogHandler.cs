using UnityEngine;

public class LogHandler : MonoBehaviour
{
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (logString.Contains("Player Joined: [Player:2]"))
        {
            Debug.Log("Triggering action based on player join log.");
            TriggerObstacleSpawning();
        }
    }

    void TriggerObstacleSpawning()
    {
        // Call your method to start obstacle spawning
        ObstacleSpawner obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        if (obstacleSpawner != null)
        {
            obstacleSpawner.StartSpawning();
        }
        else
        {
            Debug.LogWarning("ObstacleSpawner not found!");
        }
    }
}
