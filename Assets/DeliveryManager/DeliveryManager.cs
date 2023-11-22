using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] DeliveryRecipeSOList deliveryRecipeSOList;
    private List<DeliveryRecipeSO> waitingRecipeSOList;
    int waitingRecipesMax = 4;
    float spawnRecipeTimer = 4f;
    float spawnRecipeTimerMax = 4f;
    int successfulRecipesAmount;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
        waitingRecipeSOList = new List<DeliveryRecipeSO>();
    }

    private void Update()
    {
        if (!IsServer) return;
        TrySpawnRecipe();
    }



    private void TrySpawnRecipe()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (KitchenGameManager.Instance.IsGamePlaying() && spawnRecipeTimer <= 0f && waitingRecipeSOList.Count < waitingRecipesMax)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            AddRandomRecipeToWaitingList();
        }
    }

    private void AddRandomRecipeToWaitingList()
    {
        int waitingRecipeSOIndex = UnityEngine.Random.Range(0, deliveryRecipeSOList.GetDeliveryRecipeSOList().Count);
        DeliveryRecipeSO waitingRecipeSO = deliveryRecipeSOList.GetDeliveryRecipeSOList()[waitingRecipeSOIndex];
        SpawnNewWaitingRecipeClientRpc(waitingRecipeSOIndex);

    }
    [ClientRpc]
    void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {
        DeliveryRecipeSO waitingRecipeSO = deliveryRecipeSOList.GetDeliveryRecipeSOList()[waitingRecipeSOIndex];
        waitingRecipeSOList.Add(waitingRecipeSO);
        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }



    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = waitingRecipeSOList.Count - 1; i >= 0; i--)
        {
            DeliveryRecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (CheckIfRecipeMatchesPlate(waitingRecipeSO, plateKitchenObject))
            {
                DeliverCorrectRecipeServerRpc(i);
                return;
            }
        }
        DeliverIncorrectRecipeServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    void DeliverIncorrectRecipeServerRpc()
    {
        DeliverIncorrectRecipeClientRpc();
    }
    [ClientRpc]
    void DeliverIncorrectRecipeClientRpc()
    {
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
    [ServerRpc(RequireOwnership = false)]
    void DeliverCorrectRecipeServerRpc(int waitingRecipeSOIndex)
    {
        DeliverCorrectRecipeClientRpc(waitingRecipeSOIndex);
    }
    [ClientRpc]
    void DeliverCorrectRecipeClientRpc(int waitingRecipeSOIndex)
    {
        successfulRecipesAmount++;
        spawnRecipeTimer = spawnRecipeTimerMax;
        waitingRecipeSOList.RemoveAt(waitingRecipeSOIndex);
        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
    }

    private bool CheckIfRecipeMatchesPlate(DeliveryRecipeSO waitingRecipeSO, PlateKitchenObject plateKitchenObject)
    {
        if (waitingRecipeSO.kitchenObjectSOList.Count != plateKitchenObject.GetKitchenObjectSOList().Count)
            return false;

        foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
        {
            if (!plateKitchenObject.ContainsKitchenObject(recipeKitchenObjectSO))
                return false;
        }

        return true;
    }
    public List<DeliveryRecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
