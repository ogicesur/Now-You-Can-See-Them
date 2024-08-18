using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private AudioClip collisionClip; // Expose AudioClip in the Inspector
    private TextMeshProUGUI scoreText;
    private static int score = 0;

    private void Start()
    {
        // Initialize the scoreText reference
        GameObject scoreTextObject = GameObject.Find("ScoreText1");
        if (scoreTextObject != null)
        {
            scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        }

        if (scoreText == null)
        {
            Debug.LogWarning("ScoreText is not assigned. Please assign it in the inspector.");
        }

        // Update initial score display
        UpdateScoreText();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            Debug.Log("Collision with obstacle!");

            // Increase score and update text
            score += 1;
            UpdateScoreText();

            // Play the collision sound if an AudioClip is assigned
            if (collisionClip != null)
            {
                AudioSource.PlayClipAtPoint(collisionClip, transform.position);
            }
            else
            {
                Debug.LogWarning("No AudioClip assigned for collision sound.");
            }

            // Destroy the obstacle
            Destroy(collision.gameObject);

            // Trigger Arduino vibration if the controller instance exists
            if (ArduinoController.Instance != null)
            {
                ArduinoController.Instance.TriggerVibration();
            }
            else
            {
                Debug.LogWarning("ArduinoController instance not found.");
            }
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}
