using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{
    void Start()
    {
        KitchenGameMultiplayer.Instance.OnTryingToJoinGame += ShowUI;
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame += HideUI;
        Hide();
    }

    private void HideUI(object sender, EventArgs e)
    {
        Hide();
    }

    private void ShowUI(object sender, EventArgs e)
    {
        Show();
    }

    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnTryingToJoinGame -= ShowUI;
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= HideUI;
    }
}
