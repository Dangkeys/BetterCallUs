using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI recipeNameText;
    [SerializeField] Transform iconContainer;
    [SerializeField] Transform iconTemplate;
    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeSO(DeliveryRecipeSO deliveryRecipeSO)
    {
        recipeNameText.text = deliveryRecipeSO.GetRecipeName();
        foreach(Transform child in iconContainer)
        {
            if(child != iconTemplate)
                Destroy(gameObject);
        }
        foreach(KitchenObjectSO kitchenObjectSO in deliveryRecipeSO.kitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.GetSprite();
        }
    }

    
}
