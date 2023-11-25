using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseMultiplayerUI : MonoBehaviour
{
    private void Start() {
        KitchenGameManager.Instance.OnMultiplayerGamePaused += (object sender, EventArgs e) => {
            Show();
        };
        KitchenGameManager.Instance.OnMultiplayerGameUnPaused += (object sender, EventArgs e) => {
            Hide();
        };
        Hide();
    }
    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
