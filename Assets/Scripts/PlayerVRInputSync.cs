using UnityEngine;
using Photon.Pun;
using System;

public class PlayerVRInputSync : MonoBehaviourPunCallbacks, IPunObservable
{
    private PhotonView photonView;
    private InputState inputState;

    [Serializable]
    public struct InputState
    {
        public Vector3 position;
        public Quaternion rotation;
        public float triggerLeft;
        public float triggerRight;
        public bool buttonA;
        public bool buttonB;
    }

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView == null)
        {
            Debug.LogError("PhotonView component missing.");
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            CaptureInput();
        }
        else
        {
            ApplyInput();
        }
    }

    private void CaptureInput()
    {
        // Capture input data from VR system
        inputState.position = transform.position;
        inputState.rotation = transform.rotation;
        inputState.triggerLeft = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        inputState.triggerRight = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        inputState.buttonA = OVRInput.Get(OVRInput.Button.One);
        inputState.buttonB = OVRInput.Get(OVRInput.Button.Two);
    }

    private void ApplyInput()
    {
        // Apply input state to the player's transform for remote players
        transform.position = inputState.position;
        transform.rotation = inputState.rotation;
    }

    // Serialize and Deserialize data to synchronize across the network
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send data to other players
            stream.SendNext(inputState.position);
            stream.SendNext(inputState.rotation);
            stream.SendNext(inputState.triggerLeft);
            stream.SendNext(inputState.triggerRight);
            stream.SendNext(inputState.buttonA);
            stream.SendNext(inputState.buttonB);
        }
        else
        {
            // Receive data from other players
            inputState.position = (Vector3)stream.ReceiveNext();
            inputState.rotation = (Quaternion)stream.ReceiveNext();
            inputState.triggerLeft = (float)stream.ReceiveNext();
            inputState.triggerRight = (float)stream.ReceiveNext();
            inputState.buttonA = (bool)stream.ReceiveNext();
            inputState.buttonB = (bool)stream.ReceiveNext();
        }
    }
}
