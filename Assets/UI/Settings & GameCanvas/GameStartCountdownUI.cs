using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    const string NUMBER_POPUP = "NumberPopup";
    Animator animator;
    [SerializeField] TextMeshProUGUI countdownText;
    int previousCountdownNumber;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void Start() {
        KitchenGameManager.Instance.OnStateChanged += ShowCountdownUI;
        Show(false);
    }

    private void Update() {
        int countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GameStartCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();
        if(previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound();
        }
    }
    private void ShowCountdownUI(object sender, EventArgs e)
    {
        Show(KitchenGameManager.Instance.IsCountdownState());

    }
    void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }

}
