using System;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            if (player.HasKitchenObject())
                TryHandlePlate(player);
            else
                GetKitchenObject().SetKitchenObjectParent(player);
        }
        else
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
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
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }

}
