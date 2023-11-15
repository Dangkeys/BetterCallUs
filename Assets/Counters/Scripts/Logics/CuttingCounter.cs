using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] CuttingRecipeSO[] cuttingRecipeSOArray;
    int cuttingProgress;
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            if (player.HasKitchenObject()) return;
            GetKitchenObject().SetKitchenObjectParent(player);
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                progressNormalized = 0
            });
        }
        else
        {
            if (!player.HasKitchenObject()) return;
            player.GetKitchenObject().SetKitchenObjectParent(this);
            cuttingProgress = GetKitchenObject().CuttingProgress;
            if (!HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) return;
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / GetCuttingProgressMax(GetKitchenObject().GetKitchenObjectSO())
            });
        }
    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            if (player.HasKitchenObject()) return;
            if (!HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) return;
            OnCut?.Invoke(this, EventArgs.Empty);
            GetKitchenObject().IncrementCuttingProgress();
            cuttingProgress = GetKitchenObject().CuttingProgress;

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / GetCuttingProgressMax(GetKitchenObject().GetKitchenObjectSO())
            });
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputkitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputkitchenObjectSO, this);
            }
        }
    }
    private int GetCuttingProgressMax(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetCuttingRecipeSOWithInput(inputKitchenObjectSO).cuttingProgressMax;
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetCuttingRecipeSOWithInput(inputKitchenObjectSO) != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO == null)
            return null;
        else
            return cuttingRecipeSO.output;
    }
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (inputKitchenObjectSO == cuttingRecipeSO.input)
                return cuttingRecipeSO;
        }
        return null;
    }
}
