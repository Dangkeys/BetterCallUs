using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    private void Awake() {
        playButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });
        quitButton.onClick.AddListener(() =>{
            Debug.Log("Quit!!");
            Application.Quit();
        });
        Time.timeScale = 1f;
    }
}
