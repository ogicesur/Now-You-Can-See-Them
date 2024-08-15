using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Master");

        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions() { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        // Instantiate the player prefab
        GameObject player = PhotonNetwork.Instantiate("aaa", new Vector3(1, 1, 0), Quaternion.identity, 0);
        if (player != null)
        {
            Debug.Log("Player instantiated successfully");

            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                // Enable local player components (OVRManager, input handling, etc.)
                OVRManager ovrManager = player.GetComponentInChildren<OVRManager>();
                if (ovrManager != null)
                {
                    ovrManager.enabled = true;
                }
                else
                {
                    Debug.LogError("OVRManager not found in the instantiated player prefab.");
                }

                PlayerVRInputSync inputSync = player.GetComponent<PlayerVRInputSync>();
                if (inputSync != null)
                {
                    inputSync.enabled = true;
                }
                else
                {
                    Debug.LogError("PlayerVRInputSync script not found in the instantiated player prefab.");
                }
            }
            else if (photonView == null)
            {
                Debug.LogError("PhotonView component missing on player prefab.");
            }
        }
        else
        {
            Debug.LogError("Player instantiation failed");
        }
    }
}