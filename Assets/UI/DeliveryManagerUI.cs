using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] Transform recipeTemplate;
    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += OnRecipeCompleted;
        DeliveryManager.Instance.OnRecipeSpawned += OnRecipeSpawned;
        UpdateVisual();
    }

    private void OnRecipeSpawned(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    void OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }
    void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child != recipeTemplate)
                Destroy(child.gameObject);
        }
        foreach (DeliveryRecipeSO deliveryRecipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(deliveryRecipeSO);
        }
    }

}
