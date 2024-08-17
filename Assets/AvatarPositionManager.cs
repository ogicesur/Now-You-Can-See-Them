using UnityEngine;
using Oculus.Avatar2; // Ensure this namespace is included for Meta Avatars

public class AvatarPositionManager : MonoBehaviour
{
    public float distanceBetweenAvatars = 2.0f;

    private OvrAvatarEntity avatar1;
    private OvrAvatarEntity avatar2;

    void Start()
    {
        // Find the Meta Avatars, you can adjust this logic based on how your avatars are named or instantiated
        avatar1 = FindAvatar("Avatar1"); // Replace "Avatar1" with the actual name or logic to find your avatar
        avatar2 = FindAvatar("Avatar2"); // Replace "Avatar2" with the actual name or logic to find your avatar

        if (avatar1 != null && avatar2 != null)
        {
            PositionAvatars(avatar1, avatar2);
        }
        else
        {
            Debug.Log("One or both avatars are missing.");
        }
    }

    private OvrAvatarEntity FindAvatar(string avatarName)
    {
        // Implement logic to find the correct OvrAvatarEntity object
        // For example, you might search by tag, name, or any custom identifier
        GameObject avatarObj = GameObject.Find(avatarName);
        if (avatarObj != null)
        {
            return avatarObj.GetComponent<OvrAvatarEntity>();
        }
        return null;
    }

    public void PositionAvatars(OvrAvatarEntity avatar1, OvrAvatarEntity avatar2)
    {
        if (avatar1 == null || avatar2 == null) return;

        // Get the root transform of the avatars
        Transform transform1 = avatar1.transform;
        Transform transform2 = avatar2.transform;

        // Calculate the midpoint between the two avatars
        Vector3 midpoint = (transform1.position + transform2.position) / 2;

        // Calculate the direction vectors from the midpoint to each avatar
        Vector3 directionToAvatar1 = (transform1.position - midpoint).normalized;
        Vector3 directionToAvatar2 = (transform2.position - midpoint).normalized;

        // Set the positions so that they are at the desired distance from each other
        transform1.position = midpoint + directionToAvatar1 * (distanceBetweenAvatars / 2);
        transform2.position = midpoint + directionToAvatar2 * (distanceBetweenAvatars / 2);

        // Rotate each avatar to face the other
        transform1.LookAt(transform2);
        transform2.LookAt(transform1);

        // If there's a network component that needs updating, ensure it's done here
        // Example:
        // avatar1.NetworkTransform.Teleport(transform1.position, transform1.rotation);
        // avatar2.NetworkTransform.Teleport(transform2.position, transform2.rotation);
    }
}
