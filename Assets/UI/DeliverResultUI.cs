using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliverResultUI : MonoBehaviour
{
    const string POP_UP = "Popup";
    Animator animator;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] Color succesColor;
    [SerializeField] Color failedColor;
    [SerializeField] Sprite successSprite;
    [SerializeField] Sprite failedSprite;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += (object sender, EventArgs e) => {
            gameObject.SetActive(true);
            animator.SetTrigger(POP_UP);
            backgroundImage.color = succesColor;
            iconImage.sprite = successSprite;
            messageText.text = "DELIVERY\nSUCCESS";
        };
        DeliveryManager.Instance.OnRecipeFailed += (object sender, EventArgs e) => {
            gameObject.SetActive(true);
            animator.SetTrigger(POP_UP);
            backgroundImage.color = failedColor;
            iconImage.sprite = failedSprite;
            messageText.text = "DELIVERY\nFAILED";
        };
        gameObject.SetActive(false);
    }
}
