using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI recipesDevliverdText;
    [SerializeField] Button tryAgainButton;
    void Awake()
    {
        tryAgainButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += ShowGameOverUI;
        Show(false);
    }



    private void ShowGameOverUI(object sender, EventArgs e)
    {
        Show(KitchenGameManager.Instance.IsGameOverState());
    }
    void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
        if (isShow)
        {
            recipesDevliverdText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
            tryAgainButton.Select();
        }

    }
}
