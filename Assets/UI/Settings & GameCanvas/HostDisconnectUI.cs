using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectUI : MonoBehaviour
{

    [SerializeField] Button tryAgainButton;
    void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        Show(false);
    }
    void OnClientDisconnect(ulong clientId)
    {
        if (clientId == NetworkManager.ServerClientId)
            Show(true);
    }
    void Awake()
    {
        tryAgainButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
    void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
        if (isShow)
        {
            tryAgainButton.Select();
        }

    }
    void OnDestroy()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
    }
}
