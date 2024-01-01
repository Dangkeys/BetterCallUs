using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectSingleUI : MonoBehaviour
{
    [SerializeField] int colorId;
    [SerializeField] Image image;
    [SerializeField] GameObject selectedGameObject;
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(()=>{
            KitchenGameMultiplayer.Instance.ChangePlayerColor(colorId);
        });
    }
    void Start()
    {
        KitchenGameMultiplayer.Instance.Net_OnPlayerDataListChanged += (object sender, EventArgs e) => {
            UpdateIsSelected();
        };
        image.color = KitchenGameMultiplayer.Instance.GetPlayerColor(colorId);
        UpdateIsSelected();
    }
    void UpdateIsSelected()
    {
        if(KitchenGameMultiplayer.Instance.GetPlayerData().colorId == colorId)
        {
            selectedGameObject.SetActive(true);
        }else
        {
           selectedGameObject.SetActive(false); 
        }
    }
}
