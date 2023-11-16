using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingleUI : MonoBehaviour
{
    [SerializeField] Image iconImage;
    public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        iconImage.sprite = kitchenObjectSO.GetSprite();
    }
}
