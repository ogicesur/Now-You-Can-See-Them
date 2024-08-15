using Photon.Pun;
using UnityEngine;

public class PlayerVRInputSync : MonoBehaviourPun, IPunObservable
{
    private Transform headTransform;
    private Transform leftHandTransform;
    private Transform rightHandTransform;

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
            // The update method for remote players
            // This might be where you handle animation, audio, or other non-visual sync
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
            if (headTransform != null && leftHandTransform != null && rightHandTransform != null)
            {
                headTransform.position = (Vector3)stream.ReceiveNext();
                headTransform.rotation = (Quaternion)stream.ReceiveNext();
                leftHandTransform.position = (Vector3)stream.ReceiveNext();
                leftHandTransform.rotation = (Quaternion)stream.ReceiveNext();
                rightHandTransform.position = (Vector3)stream.ReceiveNext();
                rightHandTransform.rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }

}
