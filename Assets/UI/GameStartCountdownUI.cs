using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;
    private void Start() {
        KitchenGameManager.Instance.OnstateChanged += ShowCountdownUI;
        Show(false);
    }

    private void Update() {
        countdownText.text = Mathf.Ceil(KitchenGameManager.Instance.GameStartCountdownToStartTimer()).ToString();
    }
    private void ShowCountdownUI(object sender, EventArgs e)
    {
        Show(KitchenGameManager.Instance.IsCountdownState());

    }
    void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }

}
