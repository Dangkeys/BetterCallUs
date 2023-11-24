using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }
    NetworkVariable<State> state = new NetworkVariable<State>(State.Idle);

    [SerializeField] FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] BurningRecipeSO[] burningRecipeSOArray;
    NetworkVariable<float> fryingTimer = new NetworkVariable<float>(0f);
    NetworkVariable<float> burningTimer = new NetworkVariable<float>(0f);
    FryingRecipeSO fryingRecipeSO;
    BurningRecipeSO burningRecipeSO;
    public override void OnNetworkSpawn()
    {
        fryingTimer.OnValueChanged += (float previousValue, float newValue) =>
        {
            float fryingTimerMax = fryingRecipeSO != null ? fryingRecipeSO.fryingTimerMax : 1f;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = fryingTimer.Value / fryingTimerMax
            });
        };
        burningTimer.OnValueChanged += (float previousValue, float newValue) =>
        {
            float burningTimerMax = burningRecipeSO != null ? burningRecipeSO.burningTimerMax : 1f;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = burningTimer.Value / burningTimerMax
            });
        };
        state.OnValueChanged += (State previousState, State newState) =>
        {
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state.Value
            });

            if (state.Value == State.Idle || state.Value == State.Burned)
            {
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0
                });
            }
        };
    }

    private void FixedUpdate()
    {
        if (!IsServer) return;
        if (!HasKitchenObject()) return;
        switch (state.Value)
        {
            case State.Idle:
                break;
            case State.Frying:
                GetKitchenObject().IncrementFryingTimer(Time.deltaTime);
                fryingTimer.Value = GetKitchenObject().FryingTimer.Value;

                if (fryingTimer.Value >= fryingRecipeSO.fryingTimerMax)
                {
                    KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                    state.Value = State.Fried;
                    burningTimer.Value = 0f;
                    SetBurningRecipeSOClientRpc(
                        KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().GetKitchenObjectSO())
                        );
                }

                break;
            case State.Fried:
                GetKitchenObject().IncrementBurningTimer(Time.deltaTime);
                burningTimer.Value = GetKitchenObject().BurningTimer.Value;

                if (burningTimer.Value >= burningRecipeSO.burningTimerMax)
                {
                    burningTimer.Value = 0f;
                    KitchenObject.DestroyKitchenObject(GetKitchenObject());
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                    state.Value = State.Burned;
                }
                break;
            case State.Burned:
                break;
        }

    }
    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            if (!player.HasKitchenObject()) return;
            if (!TryHandlePlate(player)) return;
            SetStateIdleServerRpc();
        }
        else
        {
            if (!player.HasKitchenObject()) return;
            if (!HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) return;
            KitchenObject kitchenObject = player.GetKitchenObject();
            kitchenObject.SetKitchenObjectParent(this);
            InteractLogicServerRpc(
                KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.GetKitchenObjectSO())
            );
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void SetStateIdleServerRpc()
    {
        burningTimer.Value = 0f;
        state.Value = State.Idle;
    }
    [ServerRpc(RequireOwnership = false)]
    void InteractLogicServerRpc(int kitchenObjectSOIndex)
    {
        fryingTimer.Value = 0f;
        state.Value = State.Frying;
        SetFryingRecipeSOClientRpc(kitchenObjectSOIndex);
    }
    [ClientRpc]
    void SetFryingRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSO);
    }
    [ClientRpc]
    void SetBurningRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        burningRecipeSO = GetBurningRecipeSOWithInput(kitchenObjectSO);
    }
    private float GetFryingProgressMax(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetFryingRecipeSOWithInput(inputKitchenObjectSO).fryingTimerMax;
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetFryingRecipeSOWithInput(inputKitchenObjectSO) != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO == null)
            return null;
        else
            return fryingRecipeSO.output;
    }
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (inputKitchenObjectSO == fryingRecipeSO.input)
                return fryingRecipeSO;
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (inputKitchenObjectSO == burningRecipeSO.input)
                return burningRecipeSO;
        }
        return null;
    }
    public bool IsFried()
    {
        return state.Value == State.Fried;
    }
}
