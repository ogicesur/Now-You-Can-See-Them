using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NewBehaviourScript : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("It's connected to Master");

        PhotonNetwork.JoinOrCreateRoom("Room", new Photon.Realtime.RoomOptions() { MaxPlayers = 4 }, default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(1, 1, 0), Quaternion.identity, 0);
        if (player != null)
        {
            Debug.Log("Player instantiated successfully");
        }
        else
        {
            Debug.LogError("Player instantiation failed");
        }
    }

}