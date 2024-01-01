using System;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyCut;
    [SerializeField] CuttingRecipeSO[] cuttingRecipeSOArray;
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }
    int cuttingProgress;
    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            if (player.HasKitchenObject())
                TryHandlePlate(player);
            else
                GetKitchenObject().Net_SetKitchenObjectParent(player);
            SetProgressToZeroServerRpc();
        }
        else
        {
            if (!player.HasKitchenObject()) return;
            KitchenObject kitchenObject = player.GetKitchenObject();
            kitchenObject.Net_SetKitchenObjectParent(this);
            cuttingProgress = kitchenObject.CuttingProgress;
            if (!HasRecipeWithInput(kitchenObject.GetKitchenObjectSO())) return;
            InteractLogicPlaceObjectOnCounterServerRpc((float)cuttingProgress /
            GetCuttingProgressMax(kitchenObject.GetKitchenObjectSO()));
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void InteractLogicPlaceObjectOnCounterServerRpc(float progress)
    {
        InteractLogicPlaceObjectOnCounterClientRpc(progress);
    }
    [ClientRpc]
    void InteractLogicPlaceObjectOnCounterClientRpc(float progress)
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = progress
        });
    }
    [ServerRpc(RequireOwnership = false)]
    void SetProgressToZeroServerRpc()
    {
        SetProgressToZeroClientRpc();
    }
    [ClientRpc]
    private void SetProgressToZeroClientRpc()
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = 0
        });
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            if (player.HasKitchenObject()) return;
            if (!HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) return;
            CutObjectServerRpc();
            HandleOnCuttingProgressDoneServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void CutObjectServerRpc()
    {
        if (HasKitchenObject())
        {
            if (!HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) return;
            CutObjectClientRpc();
        }
    }
    [ClientRpc]
    void CutObjectClientRpc()
    {
        OnCut?.Invoke(this, EventArgs.Empty);
        OnAnyCut?.Invoke(this, EventArgs.Empty);
        GetKitchenObject().IncrementCuttingProgress();
        cuttingProgress = GetKitchenObject().CuttingProgress;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = (float)cuttingProgress / GetCuttingProgressMax(GetKitchenObject().GetKitchenObjectSO())
        });

    }
    [ServerRpc(RequireOwnership = false)]
    void HandleOnCuttingProgressDoneServerRpc()
    {
        if (HasKitchenObject())
        {
            if (!HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) return;
        }
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
        if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
        {
            KitchenObjectSO outputkitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
            KitchenObject.SpawnKitchenObject(outputkitchenObjectSO, this);
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
