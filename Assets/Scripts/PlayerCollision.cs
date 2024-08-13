using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private ArduinoController arduinoController;

    private void Start()
    {
        if (ArduinoController.Instance == null)
        {
            Debug.LogError("ArduinoController instance not found. Please ensure it is attached to a game object in the scene.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("与障碍物发生碰撞！");
            if (ArduinoController.Instance != null)
            {
                ArduinoController.Instance.TriggerVibration();
            }
            else
            {
                Debug.LogError("ArduinoController instance not found");
            }
        }
    }

}
