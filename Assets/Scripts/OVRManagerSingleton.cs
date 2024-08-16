using UnityEngine;

public class OVRManagerSingleton : MonoBehaviour
{
    private static OVRManagerSingleton instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Ensure OVRManager is attached and enabled
            OVRManager ovrManager = GetComponent<OVRManager>();
            if (ovrManager == null)
            {
                ovrManager = gameObject.AddComponent<OVRManager>();
                Debug.Log("OVRManager created and attached to singleton.");
            }
            ovrManager.enabled = true;
        }
        else
        {
            Debug.Log("Another instance of OVRManagerSingleton found, destroying this instance.");
            Destroy(gameObject);
        }
    }
}
