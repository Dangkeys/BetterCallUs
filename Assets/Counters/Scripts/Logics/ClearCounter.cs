using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if(HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                TryHandlePlate(player);
            }
            else
                GetKitchenObject().SetKitchenObjectParent(player);
        }else{
            if(!player.HasKitchenObject()) return;
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
    }
}
