using Photon.Pun;
using UnityEngine;

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
            string playerName = PhotonNetwork.NickName;

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
                    ovrManager.enabled = false;
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
                Debug.LogError("Unexpected player name. Expected 'Ogi' or 'Hui'.");
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
