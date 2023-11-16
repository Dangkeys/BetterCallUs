using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    [SerializeField] PlateKitchenObject plateKitchenObject;
    [SerializeField] List<KitchenObjectSO_GameObject> kitchenObjectSO_GameObjectList;
    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += ShowIngredientVisual;
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameobject in kitchenObjectSO_GameObjectList)
        {
            kitchenObjectSOGameobject.gameObject.SetActive(false);
        }
    }

    private void ShowIngredientVisual(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameobject in kitchenObjectSO_GameObjectList)
        {
            if (kitchenObjectSOGameobject.kitchenObjectSO == e.KitchenObjectSO)
            {
                kitchenObjectSOGameobject.gameObject.SetActive(true);
            }
        }
    }
}
