using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LocalAvatarSetup : MonoBehaviour
{
    GameObject localAvatar = null;
    [SerializeField] private AudioClip collisionClip; // Expose AudioClip in Inspector

    void Start()
    {
        StartCoroutine(SetupAvatarWhenReady());
    }

    public IEnumerator SetupAvatarWhenReady()
    {
        while (localAvatar == null)
        {
            localAvatar = GameObject.Find("LocalAvatar");

            if (localAvatar != null)
            {
                Debug.Log("Local Avatar is found");

                if (localAvatar.GetComponent<PlayerCollision>() == null)
                {
                   // localAvatar.AddComponent<PlayerCollision>();
                    Debug.Log("Script is added");
                }
            }

            yield return null;
        }

        SetupHandObjects(localAvatar);
    }

    public void SetupHandObjects(GameObject localAvatar)
    {
        Transform leftHand = localAvatar.transform.Find("Joint LeftHandWrist");
        Transform rightHand = localAvatar.transform.Find("Joint RightHandWrist");
        Debug.Log("Setup Hand Objects");

        if (leftHand != null)
        {
            Debug.Log("LeftHand is found");
            AddBoxColliderAndRigidbodyToHand(leftHand.gameObject, new Vector3(0.25f, 0.25f, 0.25f), new Vector3(0.09f, 0.02f, 0.0f));
            leftHand.AddComponent<PlayerCollision>();
        }

        if (rightHand != null)
        {
            Debug.Log("RightHand is found");
            AddBoxColliderAndRigidbodyToHand(rightHand.gameObject, new Vector3(0.25f, 0.25f, 0.25f), new Vector3(-0.17f, -0.05f, 0.02f));
            rightHand.AddComponent<PlayerCollision>();
        }
    }

    // public void AddBoxColliderAndRigidbodyToHand(GameObject hand)
    //{
    //hand.AddComponent<BoxCollider>();
    //hand.AddComponent<PlayerCollision>();


    //Debug.Log("Added Collider");

    // Add BoxCollider if not present
    public void AddBoxColliderAndRigidbodyToHand(GameObject hand, Vector3 colliderSize, Vector3 colliderPosition)
    {
        // Add BoxCollider if not present
        BoxCollider boxCollider = hand.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = hand.AddComponent<BoxCollider>();
        }

        // Adjust the size of the BoxCollider
        boxCollider.size = colliderSize; // Set the desired size here

        // Adjust the position of the BoxCollider
        boxCollider.center = colliderPosition; // Set the desired position here

        // Add Rigidbody if not present
        Rigidbody rb = hand.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = hand.AddComponent<Rigidbody>();
        }

        Debug.Log("Added Collider and Rigidbody with adjusted size and position");
    }

    public void Update()
    {
        if (localAvatar == null) return;

        Transform leftHandWrist = localAvatar.transform.Find("Joint LeftHandWrist");

        if (leftHandWrist != null)
        {
            BoxCollider boxCollider = leftHandWrist.GetComponent<BoxCollider>();

            if (boxCollider == null)
            {
                leftHandWrist.gameObject.AddComponent<BoxCollider>();
                SetupHandObjects(localAvatar);
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            Debug.Log("Collision with obstacle!");

            if (collisionClip != null)
            {
                AudioSource.PlayClipAtPoint(collisionClip, collision.contacts[0].point);
                Debug.Log("Playing collision sound.");
            }
            else
            {
                Debug.LogError("No AudioClip assigned for collision sound.");
            }

            Destroy(collision.gameObject);
        }
    }
}
