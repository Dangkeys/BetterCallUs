using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI moveUpText;
    [SerializeField] TextMeshProUGUI moveDownText;
    [SerializeField] TextMeshProUGUI moveLeftText;
    [SerializeField] TextMeshProUGUI moveRightText;
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] TextMeshProUGUI interactAltText;
    [SerializeField] TextMeshProUGUI pauseText;
    [SerializeField] TextMeshProUGUI interactGamepadText;
    [SerializeField] TextMeshProUGUI interactAltGamepadText;
    [SerializeField] TextMeshProUGUI pauseGamepadText;
    private void Start() {
        GameInput.Instance.OnBindingRebind += (object sender, EventArgs e) => {UpdateVisual();};
        KitchenGameManager.Instance.OnStateChanged += (object sender, EventArgs e) => {
            if(KitchenGameManager.Instance.IsCountdownState())
                Hide();
        };
        UpdateVisual();
        Show();
    }

    void UpdateVisual()
    {

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        interactGamepadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Interact);
        interactAltGamepadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_InteractAlternate);
        pauseGamepadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Pause);
    }
    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
