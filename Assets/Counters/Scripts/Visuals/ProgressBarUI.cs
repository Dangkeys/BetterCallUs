using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] GameObject hasProgressGameObject;
    IHasProgress hasProgress;
    [SerializeField] Image barImage;
    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if(hasProgress == null) 
        {
            Debug.LogError("GameObject " + hasProgressGameObject + " doesnt have IHasProgress component!");
        }
        hasProgress.OnProgressChanged += ShowBarUI;
        barImage.fillAmount = 0f;
        Show(false);
    }

    private void ShowBarUI(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Show(false);
        }
        else
        {
            Show(true);
        }
    }
    private void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}
