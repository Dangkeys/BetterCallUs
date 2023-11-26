using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingCharacterUI : MonoBehaviour
{
    [SerializeField] Button readyButton;
    void Awake()
    {
        readyButton.onClick.AddListener(() => {
            CharacterSelectReady.Instance.SetPlayerReady();
            Debug.Log("Im ready");
        });
    }
}
