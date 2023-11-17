using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu()]
public class DeliveryRecipeSOList : ScriptableObject
{
    [SerializeField] List<DeliveryRecipeSO> deliveryRecipeSOList;
    public List<DeliveryRecipeSO> GetDeliveryRecipeSOList()
    {
        return deliveryRecipeSOList;
    }
}
