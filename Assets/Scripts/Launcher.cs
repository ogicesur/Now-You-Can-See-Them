using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public Transform playerSlot1; // Assign in the Inspector
    public Transform playerSlot2; // Assign in the Inspector

    private void Start()
    {
        // Connect to Photon if not already connected
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting to Photon...");
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log("Already connected to Photon.");
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("Already in a room. Proceeding with networked instantiation.");
                AssignPlayerToSlot();
            }
            else
            {
                Debug.LogWarning("Not connected to a room. Instantiating prefab manually for testing.");
                InstantiatePrefabManually();
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Current player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        AssignPlayerToSlot();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room: " + message);
    }

    private void AssignPlayerToSlot()
    {
        // Ensure that the slots are assigned
        if (playerSlot1 == null || playerSlot2 == null)
        {
            Debug.LogError("One or more player slots are not assigned. Please check the Inspector.");
            return;
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Transform assignedSlot = GetAvailableSlot();
            if (assignedSlot == null)
            {
                Debug.LogError("No available player slot found. Ensure the slot transforms are assigned in the Inspector.");
                return;
            }

            GameObject player = PhotonNetwork.Instantiate("aaa", assignedSlot.position, assignedSlot.rotation, 0);
            if (player == null)
            {
                Debug.LogError("Failed to instantiate player prefab. Ensure the prefab name is correct and located in a Resources folder.");
                return;
            }

            Debug.Log("Player instantiated successfully");

            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView == null)
            {
                Debug.LogError("PhotonView component missing on player prefab.");
                return;
            }

            if (photonView.IsMine)
            {
                EnableLocalPlayerComponents(player);
            }
        }
    }


    private Transform GetAvailableSlot()
    {
        Debug.Log("Checking available slots...");
        Debug.Log("playerSlot1 is: " + (playerSlot1 != null ? "Assigned" : "Not Assigned"));
        Debug.Log("playerSlot2 is: " + (playerSlot2 != null ? "Assigned" : "Not Assigned"));

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Assigning player to Slot 1");
            return playerSlot1;
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Assigning player to Slot 2");
            return playerSlot2;
        }
        else
        {
            Debug.LogError("Unexpected player count. No slots available.");
            return null;
        }
    }

    private void EnableLocalPlayerComponents(GameObject player)
    {
        if (player.GetComponent<PhotonView>().IsMine)
        {
            // Enable VR input control for the local player
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
        else
        {
            Debug.Log("This player object is not owned by the local player.");
        }
    }


    private void InstantiatePrefabManually()
    {
        GameObject testPlayer = Instantiate(Resources.Load("aaa"), Vector3.zero, Quaternion.identity) as GameObject;
        if (testPlayer == null)
        {
            Debug.LogError("Manual instantiation of 'aaa' failed. Check the prefab and path.");
        }
        else
        {
            Debug.Log("Manual instantiation of 'aaa' succeeded.");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} has left the room.");
        // Additional handling logic for when a player leaves the room
    }
}
