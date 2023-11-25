using System;

using UnityEngine;

public class WaitingForOtherPlayerUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameManager.Instance.OnLocalPlayerReadyChanged += (object sender, EventArgs e) =>
        {
            if (KitchenGameManager.Instance.IsLocalPlayerReady)
                Show();
        };
        KitchenGameManager.Instance.OnStateChanged += (object sender, EventArgs e) => {
            if(KitchenGameManager.Instance.IsCountdownState())
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
