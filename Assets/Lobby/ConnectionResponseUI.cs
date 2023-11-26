using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionResponseUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] Button closeButton;
    void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }
    void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame += ShowUI;
        Hide();
    }

    private void ShowUI(object sender, EventArgs e)
    {
        Show();
        messageText.text = NetworkManager.Singleton.DisconnectReason;
        if(messageText.text == "")
        {
            messageText.text = "Failed to connect";
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
    private void OnDestroy() {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= ShowUI;
    }
}
