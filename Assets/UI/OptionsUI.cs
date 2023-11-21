using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance {get; private set;}
    [SerializeField] Button soundEffectsButton;
    [SerializeField] TextMeshProUGUI soundEffectsText;
    [SerializeField] Button musicButton;
    [SerializeField] TextMeshProUGUI musicText;
    [SerializeField] Button closeButton;
    [SerializeField] Button moveUpButton;
    [SerializeField] Button moveDownButton;
    [SerializeField] Button moveLeftButton;
    [SerializeField] Button moveRightButton;
    [SerializeField] Button interactButton;
    [SerializeField] Button interactAltButton;
    [SerializeField] Button pauseButton;
    [SerializeField] TextMeshProUGUI moveUpText;
    [SerializeField] TextMeshProUGUI moveDownText;
    [SerializeField] TextMeshProUGUI moveLeftText;
    [SerializeField] TextMeshProUGUI moveRightText;
    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] TextMeshProUGUI interactAltText;
    [SerializeField] TextMeshProUGUI pauseText;
    [SerializeField] Transform pressToRebindKeyTransform;

    void Awake()
    {
        Instance = this;
        soundEffectsButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(()=>{
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() => {
            Show(false);
        });
        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up);});
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down);});
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left);});
        moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right);});
        interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact);});
        interactAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate);});
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause);});
    }
    void Start()
    {
        KitchenGameManager.Instance.OnGameUnPaused += HideUI;
        UpdateVisual();
        Show(false);
        HidePressToRebindKey();
    }

    private void HideUI(object sender, EventArgs e)
    {
        Show(false);
    }

    void UpdateVisual()
    {
        soundEffectsText.text =  "Sound Effects: "+ Mathf.Round(SoundManager.Instance.GetVolume() * 10f).ToString();
        musicText.text = "Music: "+ Mathf.Round(MusicManager.Instance.GetVolume() * 10f).ToString();
        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    } 
    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
    void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }
    void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }
    void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () => {
            HidePressToRebindKey();
            UpdateVisual();
            });
    }
}
