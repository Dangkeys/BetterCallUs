using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] DeliveryRecipeSOList deliveryRecipeSOList;
    private List<DeliveryRecipeSO> waitingRecipeSOList;
    int waitingRecipesMax = 4;
    float spawnRecipeTimer;
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
        TrySpawnRecipe();
    }



    private void TrySpawnRecipe()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (waitingRecipeSOList.Count >= waitingRecipesMax) return;
            AddRandomRecipeToWaitingList();
        }
    }

    private void AddRandomRecipeToWaitingList()
    {
        DeliveryRecipeSO waitingRecipeSO = GetRandomRecipeFromList();
        Debug.Log(waitingRecipeSO.GetRecipeName());
        waitingRecipeSOList.Add(waitingRecipeSO);
        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }

    private DeliveryRecipeSO GetRandomRecipeFromList()
    {
        List<DeliveryRecipeSO> recipeList = deliveryRecipeSOList.GetDeliveryRecipeSOList();
        return recipeList[UnityEngine.Random.Range(0, recipeList.Count)];
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = waitingRecipeSOList.Count - 1; i >= 0; i--)
        {
            DeliveryRecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (CheckIfRecipeMatchesPlate(waitingRecipeSO, plateKitchenObject))
            {
                successfulRecipesAmount++;
                Debug.Log("Player delivered the correct recipe");
                waitingRecipeSOList.RemoveAt(i);
                OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                return;
            }
        }

        Debug.Log("Player did not deliver a correct recipe");
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
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
