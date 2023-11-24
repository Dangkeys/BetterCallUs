using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnplateSpawned;
    public event EventHandler OnplateRemoved;
    [SerializeField] KitchenObjectSO platekitchenObjectSO;
    float spawnPlateTimer;
    float spawnPlateTimerMax = 4f;
    int platesSpawnAmount;
    int platesSpawnAmountMax = 4;
    private void Update()
    {
        if (!IsServer) return;
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0;
            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnAmount < platesSpawnAmountMax)
            {
                SpawnPlateServerRpc();
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void SpawnPlateServerRpc()
    {
        SpawnPlateClientRpc();
    }
    [ClientRpc()]
    void SpawnPlateClientRpc()
    {
        platesSpawnAmount++;
        OnplateSpawned?.Invoke(this, EventArgs.Empty);

    }
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawnAmount > 0)
            {
                KitchenObject.SpawnKitchenObject(platekitchenObjectSO, player);
                InteractLogicServerRpc();
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    [ClientRpc]
    void InteractLogicClientRpc()
    {
        platesSpawnAmount--;
        OnplateRemoved?.Invoke(this, EventArgs.Empty);
    }
}
