using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] CuttingRecipeSO[] cuttingRecipeSOArray;
    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            if (player.HasKitchenObject()) return;
            GetKitchenObject().SetKitchenObjectParent(player);
        }
        else
        {
            if (!player.HasKitchenObject()) return;
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            if (player.HasKitchenObject()) return;
            if(!HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) return;
            KitchenObjectSO outputkitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(outputkitchenObjectSO, this);
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (inputKitchenObjectSO == cuttingRecipeSO.input)
                return true;
        }
        return false;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (inputKitchenObjectSO == cuttingRecipeSO.input)
                return cuttingRecipeSO.output;
        }
        return null;
    }
}
