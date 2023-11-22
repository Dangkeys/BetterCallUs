using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    // Start is called before the first frame update

    //the code below need to refactor
    public int CuttingProgress { get; private set; }
    public float FryingTimer { get; private set; }
    public float BurningTimer { get; private set; }
    //the upper code need to refactor

    [SerializeField] KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;
    public void IncrementCuttingProgress()
    {
        CuttingProgress++;
    }
    public void IncrementFryingTimer(float deltaTime)
    {
        FryingTimer += deltaTime;
    }
    public void IncrementBurningTimer(float deltaTime)
    {
        BurningTimer += deltaTime;
    }
    public void ClearFryingTimer()
    {
        FryingTimer = 0;
    }
    public void ClearBurningTimer()
    {
        BurningTimer = 0;
    }
    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }


    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IkitchenObjectParent already has a kitchenObject!");
        }
        kitchenObjectParent.SetKitchenObject(this);

        // transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        // transform.localPosition = Vector3.zero;
    }
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }
    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
    }

}
