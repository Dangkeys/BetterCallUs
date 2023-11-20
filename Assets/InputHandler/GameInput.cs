using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    public event EventHandler OnPauseAction;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        if (Instance != null) { Debug.LogError("There is more than 1 GameInput"); return; }
        Instance = this;
        playerInputActions = new PlayerInputActions();
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
}
