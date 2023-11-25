using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button optionsButton;
    void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() =>
        {
            Hide();
            OptionsUI.Instance.Show(Show);
        });
    }
    void Start()
    {
        KitchenGameManager.Instance.OnLocalGamePaused += ShowPausedUI;
        KitchenGameManager.Instance.OnLocalGameUnPaused += HidePausedUI;
        Hide();
    }

    private void HidePausedUI(object sender, EventArgs e)
    {
        Hide();
    }

    private void ShowPausedUI(object sender, EventArgs e)
    {
        Show();
    }

    void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
