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

        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        // Assign the player name based on the number of players in the room
        string playerName = PhotonNetwork.CurrentRoom.PlayerCount == 1 ? "Ogi" : "Hui";
        PhotonNetwork.NickName = playerName;

        Debug.Log($"{playerName} has joined the room.");

        // Instantiate the player prefab
        GameObject player = PhotonNetwork.Instantiate("aaa", new Vector3(1, 1, 0), Quaternion.identity, 0);
        if (player != null)
        {
            Debug.Log("Player instantiated successfully");

            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine)
            {
                if (playerName == "Ogi")
                {
                    // Enable local player (Ogi) components (OVRManager, input handling, etc.)
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
                else if (playerName == "Hui")
                {
                    // Disable OVRManager for remote player (Hui)
                    OVRManager ovrManager = player.GetComponentInChildren<OVRManager>();
                    if (ovrManager != null)
                    {
                        ovrManager.enabled = true;
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
