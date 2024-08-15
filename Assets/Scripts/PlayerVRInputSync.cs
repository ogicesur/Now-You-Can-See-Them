using Photon.Pun;
using UnityEngine;

public class PlayerVRInputSync : MonoBehaviourPun, IPunObservable
{
    private Transform headTransform;
    private Transform leftHandTransform;
    private Transform rightHandTransform;

    private Vector3 headPosition;
    private Quaternion headRotation;
    private Vector3 leftHandPosition;
    private Quaternion leftHandRotation;
    private Vector3 rightHandPosition;
    private Quaternion rightHandRotation;

    void Start()
    {
        if (photonView.IsMine)
        {
            // Find the relevant VR components in the OVR rig
            headTransform = transform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");
            leftHandTransform = transform.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor");
            rightHandTransform = transform.Find("OVRCameraRig/TrackingSpace/RightHandAnchor");

            if (headTransform == null || leftHandTransform == null || rightHandTransform == null)
            {
                Debug.LogError("VR components (head/hand transforms) not found. Ensure the structure is correct.");
            }
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            // Update remote VR components
            if (headTransform != null && leftHandTransform != null && rightHandTransform != null)
            {
                headTransform.position = headPosition;
                headTransform.rotation = headRotation;
                leftHandTransform.position = leftHandPosition;
                leftHandTransform.rotation = leftHandRotation;
                rightHandTransform.position = rightHandPosition;
                rightHandTransform.rotation = rightHandRotation;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send local player data to the network
            if (headTransform != null && leftHandTransform != null && rightHandTransform != null)
            {
                stream.SendNext(headTransform.position);
                stream.SendNext(headTransform.rotation);
                stream.SendNext(leftHandTransform.position);
                stream.SendNext(leftHandTransform.rotation);
                stream.SendNext(rightHandTransform.position);
                stream.SendNext(rightHandTransform.rotation);
            }
        }
        else
        {
            // Receive remote player data from the network
            headPosition = (Vector3)stream.ReceiveNext();
            headRotation = (Quaternion)stream.ReceiveNext();
            leftHandPosition = (Vector3)stream.ReceiveNext();
            leftHandRotation = (Quaternion)stream.ReceiveNext();
            rightHandPosition = (Vector3)stream.ReceiveNext();
            rightHandRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
