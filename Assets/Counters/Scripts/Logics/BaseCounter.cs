using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;
    private KitchenObject kitchenObject;
    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseCounter.Interact()");
    }
    public bool TryHandlePlate(Player player)
    {
        if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
            if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
            {
                GetKitchenObject().DestroySelf();
                return true;
            }
        }
        else
        {
            if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
            {
                if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().DestroySelf();
                    return true;
                }
            }
        }
        return false;
    }
    public virtual void InteractAlternate(Player player)
    {
        // Debug.LogError("BaseCounter.InteractAlternate()");
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
