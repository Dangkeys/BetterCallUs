using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    const string PLAYER_PREFS_BINDINGS = "InputBindings";
    public static GameInput Instance { get; private set; }
    public event EventHandler OnPauseAction;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    private PlayerInputActions playerInputActions;
    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
    }
    private void Awake()
    {
        if (Instance != null) { Debug.LogError("There is more than 1 GameInput"); return; }
        Instance = this;
        playerInputActions = new PlayerInputActions();
        if(PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += InteractPerformed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternatePefromed;
        playerInputActions.Player.Pause.performed += PausePerformed;
    }
    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= InteractPerformed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternatePefromed;
        playerInputActions.Player.Pause.performed -= PausePerformed;
        playerInputActions.Dispose();
    }

    private void PausePerformed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternatePefromed(InputAction.CallbackContext context)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    void InteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }
    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }
    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        InputAction inputAction;
        int bindingIndex;
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }
        playerInputActions.Player.Disable();
        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(
            callback =>
            {
                callback.Dispose();
                playerInputActions.Player.Enable();
                onActionRebound();
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            }
        ).Start();
    }
}
