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
                    localAvatar.AddComponent<PlayerCollision>();
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
        hand.AddComponent<PlayerCollision>();
        Debug.Log("Added Collider");
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
