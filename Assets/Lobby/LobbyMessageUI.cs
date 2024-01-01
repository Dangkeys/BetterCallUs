using System;
using System.Collections;
using System.Collections.Generic;
using QFSW.QC.Utilities;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] Button closeButton;
    void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }
    void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame += OnFailedToJoinGame;
        KitchenGameLobby.Instance.OnCreateLobbyFailed += OnCreateLobbyStarted;
        KitchenGameLobby.Instance.OnCreateLobbyFailed += OnCreateLobbyFailed;
        KitchenGameLobby.Instance.OnJoinStarted += ShowMessageOnJoinStarted;
        KitchenGameLobby.Instance.OnJoinFailed += ShowMessageOnJoinFailed;
        KitchenGameLobby.Instance.OnQuickJoinFailed += ShowMessageOnQuickJoinFailed;
        Hide();
    }

    private void ShowMessageOnQuickJoinFailed(object sender, EventArgs e)
    {
        ShowMesaage("Could not find a Lobby to Quick join");
    }

    private void ShowMessageOnJoinFailed(object sender, EventArgs e)
    {
        ShowMesaage("Failed to join Lobby!");
    }

    private void ShowMessageOnJoinStarted(object sender, EventArgs e)
    {
        ShowMesaage("Joining Lobby...");
    }

    private void OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowMesaage("Failed to create lobby!");
    }

    private void OnCreateLobbyStarted(object sender, EventArgs e)
    {
        ShowMesaage("Creating Lobby...");
    }
    private void ShowMesaage(string message)
    {
        Show();
        messageText.text = message;
    }
    private void OnFailedToJoinGame(object sender, EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMesaage("Failed to connect");
        }
        else
        {
            ShowMesaage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    void Show()
    {
        gameObject.SetActive(true);
        closeButton.Select();
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= OnFailedToJoinGame;
        KitchenGameLobby.Instance.OnCreateLobbyFailed -= OnCreateLobbyStarted;
        KitchenGameLobby.Instance.OnCreateLobbyFailed -= OnCreateLobbyFailed;
        KitchenGameLobby.Instance.OnJoinStarted -= ShowMessageOnJoinStarted;
        KitchenGameLobby.Instance.OnJoinFailed -= ShowMessageOnJoinFailed;
        KitchenGameLobby.Instance.OnQuickJoinFailed -= ShowMessageOnQuickJoinFailed;
    }
}
