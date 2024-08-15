using UnityEngine;
using Photon.Pun;
using System;

public class PlayerVRInputSync : MonoBehaviourPunCallbacks
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
        // Add more input fields as needed
    }

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Capture input data
            // Replace with your VR input system's methods
            inputState.position = transform.position;
            inputState.rotation = transform.rotation;
            inputState.triggerLeft = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
            inputState.triggerRight = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
            inputState.buttonA = OVRInput.GetDown(OVRInput.Button.One);
            inputState.buttonB = OVRInput.GetDown(OVRInput.Button.Two);
            // Add more input capturing logic here
        }
        else
        {
            // Apply input to remote player
            // Replace with your character or object movement logic
            transform.position = inputState.position;
            transform.rotation = inputState.rotation;
            // Handle trigger values and button presses for remote player
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && photonView.IsMine)
        {
            stream.SendNext(inputState);
        }
        else
        {
            inputState = (InputState)stream.ReceiveNext();
        }
    }
}