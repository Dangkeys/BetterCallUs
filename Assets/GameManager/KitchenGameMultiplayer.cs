using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CSharp;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public const int MAX_PLAYER_AMOUNT = 4;
    private const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";
    [SerializeField] KitchenObjectSOList kitchenObjectSOList;
    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;
    public event EventHandler Net_OnPlayerDataListChanged;
    public static KitchenGameMultiplayer Instance { get; private set; }
    NetworkList<PlayerData> net_playerDataList; // this list hold everything relate to player data
    private string playerName;

    [SerializeField] List<Color> playerColorPrefsList;// this is the list of color that player can choose

    void Awake()
    {
        net_playerDataList = new NetworkList<PlayerData>();
        Instance = this;
        playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER,
        "PlayerName" + UnityEngine.Random.Range(100, 1000));
        DontDestroyOnLoad(gameObject);
        net_playerDataList.OnListChanged += Net_PlayerDataListChanged;
    }
    public string GetPlayerName()
    {
        return playerName;
    }
    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, playerName);
    }

    private void Net_PlayerDataListChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        Net_OnPlayerDataListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += Net_OnConnectionApproved;
        NetworkManager.Singleton.OnClientConnectedCallback += Net_OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += Net_OnClientDisconnect;
        NetworkManager.Singleton.StartHost();
    }
    void Net_OnClientDisconnect(ulong clientId)
    {
        for (int i = 0; i < net_playerDataList.Count; i++)
        {
            PlayerData playerData = net_playerDataList[i];
            if (playerData.clientId == clientId)
                net_playerDataList.RemoveAt(i);
        }
    }
    private void Net_OnClientConnected(ulong clientId)
    {
        net_playerDataList.Add(new PlayerData
        {
            clientId = clientId,
            colorId = GetFirstUnusedColorId()
        });
        SetPlayerNameServerRpc(GetPlayerName());
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    private void Net_OnConnectionApproved(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (SceneManager.GetActiveScene().name != Loader.Scene.CharacterSelectScene.ToString())
        {
            response.Approved = false;
            response.Reason = "Game has already started";
            return;
        }
        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_AMOUNT)
        {
            response.Approved = false;
            response.Reason = "This lobby is full";
            return;
        }
        response.Approved = true;

    }

    public void StartClient()
    {
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);
        NetworkManager.Singleton.OnClientDisconnectCallback += DisconnectCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnectedCallback;
        NetworkManager.Singleton.StartClient();
    }

    private void ClientConnectedCallback(ulong clientId)
    {
        SetPlayerNameServerRpc(GetPlayerName());
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        //set the playerdata color to match the colorId input
        PlayerData playerData = net_playerDataList[playerDataIndex];
        playerData.playerName = playerName;
        net_playerDataList[playerDataIndex] = playerData;
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        //set the playerdata color to match the colorId input
        PlayerData playerData = net_playerDataList[playerDataIndex];
        playerData.playerId = playerId;
        net_playerDataList[playerDataIndex] = playerData;
    }

    private void DisconnectCallback(ulong obj)
    {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndex(kitchenObjectSO), kitchenObjectParent.GetNetworkObject());
    }
    [ServerRpc(RequireOwnership = false)]
    void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference parentNetworkObjectReference)
    {
        KitchenObjectSO kitchenObjectSO = GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.GetPrefabTransform());
        NetworkObject kitchenObjectNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
        kitchenObjectNetworkObject.Spawn(true);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        parentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
        kitchenObject.Net_SetKitchenObjectParent(kitchenObjectParent);
    }
    public int GetKitchenObjectSOIndex(KitchenObjectSO kitchenObjectSO)
    {
        return kitchenObjectSOList.kitchenObjectSOList.IndexOf(kitchenObjectSO);
    }
    public KitchenObjectSO GetKitchenObjectSOFromIndex(int kitchenObjectSOIndex)
    {
        return kitchenObjectSOList.kitchenObjectSOList[kitchenObjectSOIndex];
    }
    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }
    [ServerRpc(RequireOwnership = false)]
    void DestroyKitchenObjectServerRpc(NetworkObjectReference networkObjectReference)
    {
        networkObjectReference.TryGet(out NetworkObject networkObject);
        KitchenObject kitchenObject = networkObject.GetComponent<KitchenObject>();
        ClearKitchenObjectOnParentClientRpc(networkObjectReference);
        kitchenObject.DestroySelf();
    }
    [ClientRpc]
    void ClearKitchenObjectOnParentClientRpc(NetworkObjectReference networkObjectReference)
    {
        networkObjectReference.TryGet(out NetworkObject networkObject);
        KitchenObject kitchenObject = networkObject.GetComponent<KitchenObject>();
        kitchenObject.ClearKitchenObjectOnParent();
    }

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        return playerIndex < net_playerDataList.Count;
    }
    public PlayerData GetPlayerDataFromPlayerIndex(int playerIndex)
    {
        return net_playerDataList[playerIndex];
    }
    public Color GetPlayerColor(int colorId)
    {
        return playerColorPrefsList[colorId];
    }
    public PlayerData GetPlayerDataFromClientId(ulong clientId)
    {
        foreach (PlayerData playerData in net_playerDataList)
        {
            if (playerData.clientId == clientId)
                return playerData;
        }
        return default;
    }
    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        for (int i = 0; i < net_playerDataList.Count; i++)
        {
            if (net_playerDataList[i].clientId == clientId)
                return i;
        }
        return -1;
    }
    public PlayerData GetPlayerData()
    {
        return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }

    public void ChangePlayerColor(int colorId)
    {
        ChangePlayerColorServerRpc(colorId);
    }
    [ServerRpc(RequireOwnership = false)]
    void ChangePlayerColorServerRpc(int colorId, ServerRpcParams serverRpcParams = default)
    {
        if (!IsColorAvailable(colorId))
            return;
        int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        //set the playerdata color to match the colorId input
        PlayerData playerData = net_playerDataList[playerDataIndex];
        playerData.colorId = colorId;
        net_playerDataList[playerDataIndex] = playerData;
    }
    bool IsColorAvailable(int colorId)
    {
        foreach (PlayerData playerData in net_playerDataList)
        {
            if (playerData.colorId == colorId)
                return false;
        }
        return true;
    }
    int GetFirstUnusedColorId()
    {
        for (int i = 0; i < playerColorPrefsList.Count; i++)
        {
            if (IsColorAvailable(i))
            {
                return i;
            }
        }
        return -1;
    }
    public void KickPlayer(ulong clientId)
    {
        NetworkManager.Singleton.DisconnectClient(clientId);
        Net_OnClientDisconnect(clientId);
    }
}

