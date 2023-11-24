using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    // Start is called before the first frame update

    //the code below need to refactor
    public int CuttingProgress { get; private set; }
    public NetworkVariable<float> FryingTimer = new NetworkVariable<float>(0f);
    public NetworkVariable<float> BurningTimer = new NetworkVariable<float>(0f);
    FollowTransform followTransform;

    protected virtual void Awake()
    {
        followTransform = GetComponent<FollowTransform>();
    }
    //the upper code need to refactor

    [SerializeField] KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;
    
    public void IncrementCuttingProgress()
    {
        CuttingProgress++;
    }
    public void IncrementFryingTimer(float deltaTime)
    {
        if(!IsServer) return;
        FryingTimer.Value += deltaTime;
    }
    public void IncrementBurningTimer(float deltaTime)
    {
        if(!IsServer) return;
        BurningTimer.Value += deltaTime;
    }
    public void ClearFryingTimer()
    {
        if(!IsServer) return;
        FryingTimer.Value = 0;
    }
    public void ClearBurningTimer()
    {
        if(!IsServer) return;
        BurningTimer.Value = 0;
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
        SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
    }
    [ServerRpc(RequireOwnership = false)]
    void SetKitchenObjectParentServerRpc(NetworkObjectReference parentNetworkObjectReference)
    {
        SetKitchenObjectParentClientRpc(parentNetworkObjectReference);
    }
    [ClientRpc]
    void SetKitchenObjectParentClientRpc(NetworkObjectReference parentNetworkObjectReference)
    {
        parentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
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

        followTransform.TargetTransform = kitchenObjectParent.GetKitchenObjectFollowTransform();
    }
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
    public void DestroySelf()
    {

        Destroy(gameObject);
    }
    public void ClearKitchenObjectOnParent()
    {
        kitchenObjectParent.ClearKitchenObject();
    }
    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
    }
    public static void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
    }

}
