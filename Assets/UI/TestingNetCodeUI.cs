using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetCodeUI : MonoBehaviour
{
  [SerializeField] Button startHostButton;
  [SerializeField] Button startClientButton;
  void Awake()
  {
    startHostButton.onClick.AddListener(()=> {
        Debug.Log("HOST");
        KitchenGameMultiplayer.Instance.StartHost();
        Hide();
    });
    startClientButton.onClick.AddListener(() => {
        Debug.Log("CLIENT");
        KitchenGameMultiplayer.Instance.StartClient();
        Hide();
    });
  }
  void Start()
  {
    startHostButton.Select();
  }
  void Hide()
  {
    gameObject.SetActive(false);
  }
    
}
