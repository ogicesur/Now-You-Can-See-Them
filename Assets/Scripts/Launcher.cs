using UnityEngine;
using Photon.Pun;

public class NewBehaviourScript : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public float yPosition;
    public float zPosition;
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
        GameObject player = PhotonNetwork.Instantiate("aaa", new Vector3(1, yPosition, zPosition), Quaternion.identity, 0);
        if (player != null)
        {
            Debug.Log("Player instantiated successfully");

            SetupPlayerComponents(player);
        }
        else
        {
            Debug.LogError("Player instantiation failed");
        }
    }

    private void SetupPlayerComponents(GameObject player)
    {
        // Handle the OVRManager, OVRCameraRig, and the avatar body
        OVRManager ovrManager = player.GetComponent<OVRManager>();
        OVRCameraRig cameraRig = player.GetComponentInChildren<OVRCameraRig>();
        Transform avatarBody = player.transform.Find("AvatarBody"); // Replace with the correct path to your avatar body

        if (photonView.IsMine)
        {
            // This is the local player, enable VR components
            ovrManager.enabled = true;
            cameraRig.gameObject.SetActive(true);

            // Initialize the local player's avatar with VR controls
            player.AddComponent<LocalPlayerController>().Initialize(cameraRig, avatarBody, photonView);
        }
        else
        {
            // This is a remote player, disable VR components
            ovrManager.enabled = false;
            cameraRig.gameObject.SetActive(false);

            // Add a component to handle synchronization of remote players
            player.AddComponent<RemotePlayerController>().Initialize(avatarBody, photonView);
        }
    }
}

public class LocalPlayerController : MonoBehaviour
{
    private OVRCameraRig cameraRig;
    private Transform avatarBody;
    private PhotonView photonView;

    public void Initialize(OVRCameraRig rig, Transform body, PhotonView view)
    {
        cameraRig = rig;
        avatarBody = body;
        photonView = view;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Update local player input and tracking
            Vector3 localPosition = cameraRig.centerEyeAnchor.localPosition;
            Quaternion localRotation = cameraRig.centerEyeAnchor.localRotation;

            // Update the avatar body with local VR tracking data
            avatarBody.localPosition = localPosition;
            avatarBody.localRotation = localRotation;

            // Synchronize the body tracking with other players
            photonView.RPC("SyncBody", RpcTarget.Others, localPosition, localRotation);
        }
    }

    [PunRPC]
    void SyncBody(Vector3 position, Quaternion rotation)
    {
        // This RPC is called on remote instances to update the avatar body position and rotation
        avatarBody.localPosition = position;
        avatarBody.localRotation = rotation;
    }
}

public class RemotePlayerController : MonoBehaviour
{
    private Transform avatarBody;
    private PhotonView photonView;

    public void Initialize(Transform body, PhotonView view)
    {
        avatarBody = body;
        photonView = view;
    }

    [PunRPC]
    void SyncBody(Vector3 position, Quaternion rotation)
    {
        // This method is called to synchronize the remote player body tracking
        avatarBody.localPosition = position;
        avatarBody.localRotation = rotation;
    }
}