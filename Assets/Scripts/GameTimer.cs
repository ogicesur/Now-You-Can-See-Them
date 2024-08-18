using UnityEngine;
using TMPro;
using Fusion;
using System.Collections.Generic;
using Fusion.Sockets;
using System;

public class GameTimer : NetworkBehaviour, INetworkRunnerCallbacks
{
    public TextMeshProUGUI timerText; // Reference to the timerText in the UI
    public float gameTime = 300f; // Total game time in seconds
    [Networked] private TickTimer networkedTime { get; set; }
    public NetworkRunner Runner; // Reference to the NetworkRunner

    private int playerCount = 0; // To track the number of players

    void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("TimerText is not assigned in the inspector!");
            return; // Exit early to avoid further errors
        }

        if (Runner == null)
        {
            // Attempt to find an existing NetworkRunner in the scene
            Runner = FindObjectOfType<NetworkRunner>();

            if (Runner == null)
            {
                Debug.LogError("NetworkRunner is not assigned or initialized and could not be found in the scene!");
                return; // Exit early to avoid further errors
            }
        }

        // Register this script as a listener for network callbacks
        Runner.AddCallbacks(this);
    }


    void Update()
    {
        if (Runner != null && Runner.IsRunning && Runner.SessionInfo.IsValid)
        {
            if (!Object.IsValid || timerText == null)
            {
                return;
            }

            if (networkedTime.ExpiredOrNotRunning(Runner))
            {
                UpdateTimerUI(0);
                return;
            }

            if (Object.HasInputAuthority)
            {
                UpdateTimerUI(networkedTime.RemainingTime(Runner).Value);
            }
        }
    }

    private void UpdateTimerUI(float remainingTime)
    {
        if (timerText == null)
        {
            Debug.LogError("TimerText is not set!");
            return;
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        if (Object.HasStateAuthority)
        {
            networkedTime = TickTimer.CreateFromSeconds(Runner, gameTime);
            Debug.Log("Timer started.");
        }
    }

    public void SyncTimer()
    {
        if (Object.HasInputAuthority)
        {
            UpdateTimerUI(networkedTime.RemainingTime(Runner).Value);
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        playerCount++;

        Debug.Log("Player joined. Total players: " + playerCount);

        if (playerCount == 2) // Start the timer when the second player joins
        {
            Debug.Log("Second player joined, starting the timer.");
            StartTimer();
        }
        else if (playerCount > 2)
        {
            SyncTimer(); // Sync timer for additional players
        }
    }

    // Required by INetworkRunnerCallbacks, but not used in this script
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject networkObject, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject networkObject, PlayerRef player) { }
}
