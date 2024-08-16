using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollision : MonoBehaviour
{
    // Reference to the AudioSource component
    private AudioSource audioSource;
    private TextMeshProUGUI scoreText;
    private int score = 0;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
        }
        else
        {
            audioSource.Play(); // This should play the audio when the game starts
            Debug.Log("Test sound played at start.");
        }
        GameObject scoreTextObject = GameObject.Find("ScoreText1");
        if (scoreTextObject != null)
        {
            scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        }

        if (scoreText == null)
        {
            Debug.LogError("ScoreText is not assigned. Please assign it in the inspector.");
        }

        // 更新初始分数显示
        UpdateScoreText();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            Debug.Log("与障碍物发生碰撞！");

            score += 1;
            UpdateScoreText();


            // Check if the AudioSource and AudioClip are correctly assigned
            if (audioSource != null)
            {
                if (audioSource.clip != null)
                {
                    Debug.Log("Playing collision sound.");
                    audioSource.PlayOneShot(audioSource.clip);
                }
                else
                {
                    Debug.LogError("AudioClip is not assigned to the AudioSource.");
                }
            }
            else
            {
                Debug.LogError("AudioSource component is missing or not assigned.");
            }

            Destroy(collision.gameObject);

            if (ArduinoController.Instance != null)
            {
                ArduinoController.Instance.TriggerVibration();
            }
            else
            {
                Debug.LogError("ArduinoController instance not found.");
            }
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
