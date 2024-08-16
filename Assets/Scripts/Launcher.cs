using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // The player prefab (should be assigned in the Inspector)

    private void Start()
    {
        // Ensure OVRManagerSingleton is initialized
        if (FindObjectOfType<OVRManagerSingleton>() == null)
        {
            Debug.LogError("OVRManagerSingleton is missing. Please ensure it's added to the scene.");
        }

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

        // Instantiate the player prefab for the local player
        InstantiateLocalPlayer();
    }

    private void InstantiateLocalPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned in the Inspector.");
            return;
        }

        // Instantiate the player prefab at a default position and rotation
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0);

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

        // Debug check to verify if this player is the local player
        if (photonView.IsMine)
        {
            Debug.Log("This is the local player.");
            EnableLocalPlayerComponents(player);
        }
        else
        {
            Debug.Log("This is a remote player.");
        }
    }

    private void EnableLocalPlayerComponents(GameObject player)
    {
        PhotonView photonView = player.GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            // Enable VR input control for the local player
            PlayerVRInputSync inputSync = player.GetComponent<PlayerVRInputSync>();
            if (inputSync != null)
            {
                inputSync.enabled = true;
                Debug.Log("PlayerVRInputSync enabled for local player.");
            }
            else
            {
                Debug.LogError("PlayerVRInputSync script not found in the instantiated player prefab.");
            }

            OVRCameraRig cameraRig = player.GetComponentInChildren<OVRCameraRig>();
            if (cameraRig != null)
            {
                cameraRig.gameObject.SetActive(true);
                Debug.Log("OVRCameraRig activated for local player.");
            }
            else
            {
                Debug.LogError("OVRCameraRig not found in the instantiated player prefab.");
            }

            // Enable OVRCustomSkeleton for the local player
            OVRCustomSkeleton customSkeleton = player.GetComponentInChildren<OVRCustomSkeleton>();
            if (customSkeleton != null)
            {
                customSkeleton.enabled = true;
                Debug.Log("OVRCustomSkeleton enabled for local player.");
            }
            else
            {
                Debug.LogError("OVRCustomSkeleton not found in the instantiated player prefab.");
            }
        }
        else
        {
            // Disable OVRCustomSkeleton for remote players
            OVRCustomSkeleton customSkeleton = player.GetComponentInChildren<OVRCustomSkeleton>();
            if (customSkeleton != null)
            {
                customSkeleton.enabled = false;
                Debug.Log("OVRCustomSkeleton disabled for remote player.");
            }
        }
    }

}
