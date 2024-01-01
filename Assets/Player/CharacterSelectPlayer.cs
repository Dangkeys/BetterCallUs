using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] int playerIndex;
    [SerializeField] GameObject readyGameObject;
    [SerializeField] PlayerVisual playerVisual;
    [SerializeField] Button kickButton;
    [SerializeField] TextMeshPro playerNameText;
    void Awake()
    {
        kickButton.onClick.AddListener(() =>
        {
            Debug.Log("Meow");
            PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            KitchenGameLobby.Instance.KickPlayer(playerData.playerId.ToString());
            KitchenGameMultiplayer.Instance.KickPlayer(playerData.clientId);
        });
    }
    void Start()
    {
        KitchenGameMultiplayer.Instance.Net_OnPlayerDataListChanged += UpdateVisual;
        CharacterSelectReady.Instance.OnReadyChanged += ShowReadyGameObject;
        UpdatePlayerVisual();
        if (NetworkManager.Singleton.IsServer)
        {
         PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex);   
            kickButton.gameObject.SetActive(true);
        }
    }

    private void UpdateVisual(object sender, EventArgs e)
    {
        UpdatePlayerVisual();
    }

    private void ShowReadyGameObject(object sender, EventArgs e)
    {
        UpdatePlayerVisual();
    }

    void Show()
    {
        gameObject.SetActive(true);
    }
    void UpdatePlayerVisual()
    {
        if (KitchenGameMultiplayer.Instance.IsPlayerIndexConnected(playerIndex))
        {
            Show();
            PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));
            playerNameText.text = playerData.playerName.ToString();
            playerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
        }
        else
        {
            Hide();
        }
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.Net_OnPlayerDataListChanged -= UpdateVisual;
    }

}
