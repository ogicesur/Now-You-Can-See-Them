using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LocalAvatarSetup : MonoBehaviour
{
    GameObject localAvatar = null;
    void Start()
    {
        StartCoroutine(SetupAvatarWhenReady());
        if (localAvatar.GetComponent<PlayerCollision>() == null)
        {
            localAvatar.AddComponent<PlayerCollision>(); // Add your custom script
            Debug.Log("Script is added");
        }
    }

    public IEnumerator SetupAvatarWhenReady()
    {
        
        
        // Wait until the local avatar is instantiated
        while (localAvatar == null)
        {
            localAvatar = GameObject.Find("LocalAvatar"); // Replace with the correct name if different
            if (localAvatar != null) {
                Debug.Log( "Local Avatar is found");
            }

            yield return null; // Wait for the next frame and check again
        }

        // Apply your changes to the local avatar
       // SetupHandObjects(localAvatar);
    }

   public void SetupHandObjects(GameObject localAvatar)
    {
        // Assuming your hands are children of the local avatar
        Transform leftHand = localAvatar.transform.Find("Joint LeftHandWrist"); // Replace with correct path
        Transform rightHand = localAvatar.transform.Find("Joint RightHandWrist");
        Debug.Log("Setup Hand Objects");

        if (leftHand != null)
        {
            Debug.Log("LeftHand is found");
            AddBoxColliderAndRigidbodyToHand(leftHand.gameObject);
        }

        if (rightHand != null)
        {
            Debug.Log("RightHand is found");
            AddBoxColliderAndRigidbodyToHand(rightHand.gameObject);
        }
    }

    public void AddBoxColliderAndRigidbodyToHand(GameObject hand)
    {
        hand.AddComponent<BoxCollider>();
        hand.AddComponent<PlayerCollision>(); // Add your custom script
        Debug.Log("Added Collider");
        /*
         * // Add a BoxCollider if one doesn't exist
        if (hand.GetComponent<BoxCollider>() == null)
        {
            hand.AddComponent<BoxCollider>();
        }

        // Add a Rigidbody if one doesn't exist
        if (hand.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = hand.AddComponent<Rigidbody>();
            rb.isKinematic = true; // Make the Rigidbody kinematic so it doesn't respond to physics forces
        }
        if (hand.GetComponent<PlayerCollision>() == null)
        {
            hand.AddComponent<PlayerCollision>(); // Add your custom script
        } */

    }

    public void Update()
    {
        // Find the "Joint LeftHandWrist" under localAvatar's transform
        Transform leftHandWrist = localAvatar.transform.Find("Joint LeftHandWrist");

        // Check if the "Joint LeftHandWrist" object exists
        if (leftHandWrist != null)
        {
            // Check if there is a BoxCollider component
            BoxCollider boxCollider = leftHandWrist.GetComponent<BoxCollider>();

            // If there is no BoxCollider, add one and set up the hand objects
            if (boxCollider == null)
            {
                leftHandWrist.gameObject.AddComponent<BoxCollider>();
                SetupHandObjects(localAvatar);
            }
        }
    }



}