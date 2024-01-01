using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button multiplayerButton;
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] Button quitButton;
    private void Awake()
    {
        multiplayerButton.onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.playMultiplayer = true;
            Loader.Load(Loader.Scene.LobbyScene);
        });
        singlePlayerButton.onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.playMultiplayer = false;
            Loader.Load(Loader.Scene.LobbyScene);
        });
        quitButton.onClick.AddListener(() =>
        {
            Debug.Log("Quit!!");
            Application.Quit();
        });
        Time.timeScale = 1f;
    }
}
