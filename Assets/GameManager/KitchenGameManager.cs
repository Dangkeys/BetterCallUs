using System;
using System.Collections.Generic;
using QFSW.QC;
using Unity.Netcode;
using UnityEngine;
public class KitchenGameManager : NetworkBehaviour
{
    public static KitchenGameManager Instance { get; private set; }
    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalGamePaused;
    public event EventHandler OnLocalGameUnPaused;
    public event EventHandler OnLocalPlayerReadyChanged;
    public event EventHandler OnMultiplayerGamePaused;
    public event EventHandler OnMultiplayerGameUnPaused;
    enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }
    NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    public bool IsLocalPlayerReady { get; private set; }
    NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false);

    NetworkVariable<float> countDownToStartTimer = new NetworkVariable<float>(3f);
    NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(0f);
    float gamePlayingTimerMax = 120f;
    public bool IsLocalGamePaused { get; private set; }
    Dictionary<ulong, bool> playerReadyDictionary;
    Dictionary<ulong, bool> playerPausedDictionary;
    bool autoTestGamePausedState;

    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
        playerPausedDictionary = new Dictionary<ulong, bool>();
    }

    void Start()
    {
        GameInput.Instance.OnPauseAction += GameInputOnPauseAction;
        GameInput.Instance.OnInteractAction += (object sender, EventArgs e) =>
        {
            if (state.Value == State.WaitingToStart)
            {
                IsLocalPlayerReady = true;
                OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
                SetPlayerReadyServerRpc();
            }
        };
    }
    private void Update()
    {
        if (!IsServer) return;
        switch (state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountDownToStart:
                countDownToStartTimer.Value -= Time.deltaTime;
                if (countDownToStartTimer.Value < 0f)
                {
                    state.Value = State.GamePlaying;
                    gamePlayingTimer.Value = gamePlayingTimerMax;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                if (gamePlayingTimer.Value < 0f)
                {
                    state.Value = State.GameOver;

                }
                break;
            case State.GameOver:
                break;
        }
    }
    void LateUpdate()
    {
        if(autoTestGamePausedState)
        {
            autoTestGamePausedState = false;
            TestGamePausedState();
        }
    }
    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += (State previousState, State newState) =>
        {
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        };
        isGamePaused.OnValueChanged += (bool previousValue, bool newValue) =>
        {
            if (isGamePaused.Value)
            {
                Time.timeScale = 0f;
                OnMultiplayerGamePaused?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Time.timeScale = 1f;
                OnMultiplayerGameUnPaused?.Invoke(this, EventArgs.Empty);
            }
        };
        if(IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += (ulong clientId) => {
                autoTestGamePausedState = true;
            };
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                // This player is NOT ready
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            state.Value = State.CountDownToStart;
        }
    }
    private void GameInputOnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        IsLocalGamePaused = !IsLocalGamePaused;
        if (IsLocalGamePaused)
        {
            PauseGameServerRpc();
            OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            UnPauseGameServerRpc();
            OnLocalGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;
        TestGamePausedState();
    }
    [ServerRpc(RequireOwnership = false)]
    void UnPauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;
        TestGamePausedState();
    }
    void TestGamePausedState()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playerPausedDictionary.ContainsKey(clientId) && playerPausedDictionary[clientId])
            {
                isGamePaused.Value = true;
                return;
            }
        }
        isGamePaused.Value = false;
    }
    public bool IsWaitingToStart(){
        return state.Value == State.WaitingToStart;
    }
    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }
    public bool IsCountdownState()
    {
        return state.Value == State.CountDownToStart;
    }
    public float GameStartCountdownToStartTimer()
    {
        return countDownToStartTimer.Value;
    }
    public bool IsGameOverState()
    {
        return state.Value == State.GameOver;
    }
    public float GetGamePlayingTimerNormalized()
    {
        return gamePlayingTimer.Value / gamePlayingTimerMax;
    }
    public bool GetGamePaused()
    {
        return isGamePaused.Value;
    }
}
