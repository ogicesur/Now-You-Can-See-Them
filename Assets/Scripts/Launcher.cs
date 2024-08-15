using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Master");

        PhotonNetwork.JoinOrCreateRoom("Room", new Photon.Realtime.RoomOptions() { MaxPlayers = 4 }, default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        // Instantiate the player prefab
        GameObject player = PhotonNetwork.Instantiate("aaa", new Vector3(1, 1, 0), Quaternion.identity, 0);
        if (player != null)
        {
            Debug.Log("Player instantiated successfully");

            // If this player is the local player, enable VR controls and other local-only components
            if (player.GetComponent<PhotonView>().IsMine)
            {
                // Enable local player components (OVRManager, input handling, etc.)
                player.GetComponent<OVRManager>().enabled = true;
                player.GetComponent<PlayerVRInputSync>().enabled = true;
            }
            else
            {
                // Disable local-specific components for remote players
                player.GetComponent<OVRManager>().enabled = false;
                player.GetComponent<PlayerVRInputSync>().enabled = false;
            }
        }
        else
        {
            Debug.LogError("Player instantiation failed");
        }
    }
}
