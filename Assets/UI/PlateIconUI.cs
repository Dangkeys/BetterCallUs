using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] PlateKitchenObject plateKitchenObject;
    [SerializeField] Transform IconTemplate;
    private void Awake() {
        IconTemplate.gameObject.SetActive(false);
    }
    private void Start() {
        plateKitchenObject.OnIngredientAdded += AddIconUI;
    }

    private void AddIconUI(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
       UpdateVisual(); 
    }
    void UpdateVisual()
    {
        foreach(Transform child in transform)
        {
            if(child != IconTemplate)
                Destroy(child.gameObject);
        }
        foreach(KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
        {
            Transform iconTransform = Instantiate(IconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
