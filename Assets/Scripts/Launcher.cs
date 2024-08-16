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

        // Instantiate player only if the player count is 1 or 2
        AssignPlayerToSlot();
    }

    private void AssignPlayerToSlot()
    {
        // Ensure that the slots are assigned
        if (playerSlot1 == null || playerSlot2 == null)
        {
            Debug.LogError("One or more player slots are not assigned. Please check the Inspector.");
            return;
        }

        Debug.Log("Assigning player to slot...");
        Transform assignedSlot = GetAvailableSlot();

        if (assignedSlot == null)
        {
            Debug.LogError("No available player slot found. Ensure the slot transforms are assigned in the Inspector.");
            return;
        }

        // Instantiate the player prefab only for the current player
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

    private Transform GetAvailableSlot()
    {
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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " has joined the room.");

        // If this is the second player joining, assign them to the second slot
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            AssignPlayerToSlot();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " has left the room.");
        // Handle player leaving (optional logic, e.g., reset slots)
    }
}
