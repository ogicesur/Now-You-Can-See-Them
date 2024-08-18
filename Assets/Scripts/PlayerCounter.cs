using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounter : MonoBehaviour
{
    public GameObject waitingText;

    void OnEnable()
    {
        // Show the waiting text
        if (waitingText != null)
        {
            waitingText.SetActive(true);
        }
    }

    void OnDisable()
    {
        // Hide the waiting text
        if (waitingText != null)
        {
            waitingText.SetActive(false);
        }
    }
}
