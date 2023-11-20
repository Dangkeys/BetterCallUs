using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button mainMenuButton;
    void Awake()
    {
        resumeButton.onClick.AddListener(()=>{
            KitchenGameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(()=>{
           Loader.Load(Loader.Scene.MainMenuScene); 
        });
    }
    void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += ShowPausedUI;
        KitchenGameManager.Instance.OnGameUnPaused += HidePausedUI;
        Show(false);
    }

    private void HidePausedUI(object sender, EventArgs e)
    {
        Show(false);
    }

    private void ShowPausedUI(object sender, EventArgs e)
    {
        Show(true);
    }

    void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}
