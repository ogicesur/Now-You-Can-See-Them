using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public string roomName = "DefaultRoom";
    public int maxPlayers = 2;

    private void Start()
    {
        ConnectToPhoton();
    }

    private void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        JoinOrCreateRoom();
    }

    private void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)maxPlayers;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + roomName);
        InstantiatePlayer();
    }

    private void InstantiatePlayer()
    {
        // Replace "PlayerPrefab" with the actual name of your player prefab in the Resources folder
        string prefabName = "PlayerPrefab";
        Vector3 spawnPosition = GetSpawnPosition();
        Quaternion spawnRotation = Quaternion.identity;

        GameObject player = PhotonNetwork.Instantiate(prefabName, spawnPosition, spawnRotation, 0);
        if (player != null)
        {
            Debug.Log("Player instantiated successfully");
        }
        else
        {
            Debug.LogError("Failed to instantiate player");
        }
    }

    private Vector3 GetSpawnPosition()
    {
        // This method should return the correct spawn position for the player.
        // For example, you could check the number of players in the room and assign different spawn points.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            return new Vector3(0, 0, 0); // Position for the first player
        }
        else
        {
            return new Vector3(2, 0, 0); // Position for the second player
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " left the room");
        // Handle the disconnection logic, like freeing up slots for new players.
    }
}
