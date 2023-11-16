using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            if(player.HasKitchenObject())
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
                OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
