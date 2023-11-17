using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DeliveryRecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> kitchenObjectSOList; 
    [SerializeField] string recipeName;
    public string GetRecipeName()
    {
        return recipeName;
    }

}
