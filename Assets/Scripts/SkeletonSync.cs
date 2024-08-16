using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkeletonSync : MonoBehaviourPun, IPunObservable
{
    private OVRCustomSkeleton customSkeleton;

    private void Start()
    {
        customSkeleton = GetComponent<OVRCustomSkeleton>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (customSkeleton == null)
            return;

        if (stream.IsWriting)
        {
            // Sending local player's skeleton data to the remote players
            foreach (Transform bone in customSkeleton.CustomBones)
            {
                if (bone != null)
                {
                    stream.SendNext(bone.localPosition);
                    stream.SendNext(bone.localRotation);
                }
                else
                {
                    stream.SendNext(Vector3.zero);
                    stream.SendNext(Quaternion.identity);
                }
            }
        }
        else
        {
            // Receiving skeleton data from the remote player
            for (int i = 0; i < customSkeleton.CustomBones.Count; i++)
            {
                Vector3 position = (Vector3)stream.ReceiveNext();
                Quaternion rotation = (Quaternion)stream.ReceiveNext();

                if (customSkeleton.CustomBones[i] != null)
                {
                    customSkeleton.CustomBones[i].localPosition = position;
                    customSkeleton.CustomBones[i].localRotation = rotation;
                }
            }
        }
    }
}
