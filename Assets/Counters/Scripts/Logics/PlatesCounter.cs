using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnplateSpawned;
    public event EventHandler OnplateRemoved;
    [SerializeField] KitchenObjectSO platekitchenObjectSO;
    float spawnPlateTimer;
    float spawnPlateTimerMax = 4f;
    int platesSpawnAmount;
    int platesSpawnAmountMax = 4;
    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        if( spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0;
            if(KitchenGameManager.Instance.IsGamePlaying() && platesSpawnAmount < platesSpawnAmountMax)
            {
                platesSpawnAmount++;
                OnplateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            if(platesSpawnAmount > 0)
            {
                platesSpawnAmount--;
                KitchenObject.SpawnKitchenObject(platekitchenObjectSO, player);
                OnplateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
