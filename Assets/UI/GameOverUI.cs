using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI  recipesDevliverdText;
        private void Start() {
        KitchenGameManager.Instance.OnstateChanged += ShowGameOverUI;
        Show(false);
    }


        
    private void ShowGameOverUI(object sender, EventArgs e)
    {
        Show(KitchenGameManager.Instance.IsGameOverState());
    }
    void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
        if(isShow)
            recipesDevliverdText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();

    }
}
