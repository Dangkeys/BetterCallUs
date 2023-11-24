using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }
    [SerializeField] List<KitchenObjectSO> validKitchenObjectSOList;
    List<KitchenObjectSO> kitchenObjectSOList;
    protected override void Awake()
    {
        base.Awake();
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) return false;
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            AddIngredientServerRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSO));
            return true;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void AddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredientClientRpc(kitchenObjectSOIndex);
    }
    [ClientRpc]
    void AddIngredientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
        {
            KitchenObjectSO = kitchenObjectSO
        });
    }
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
    public bool ContainsKitchenObject(KitchenObjectSO kitchenObjectSO)
    {
        foreach (KitchenObjectSO plateKitchenObjectSO in kitchenObjectSOList)
        {
            if (plateKitchenObjectSO == kitchenObjectSO)
            {
                return true;
            }
        }
        return false;
    }
}
