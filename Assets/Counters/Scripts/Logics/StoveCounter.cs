using System;
using System.Collections;
using System.Collections.Generic;
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
    State state;

    [SerializeField] FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] BurningRecipeSO[] burningRecipeSOArray;
    float fryingTimer;
    float burningTimer;
    FryingRecipeSO fryingRecipeSO;
    BurningRecipeSO burningRecipeSO;
    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        if (!HasKitchenObject()) return;
        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                GetKitchenObject().IncrementFryingTimer(Time.deltaTime);
                fryingTimer = GetKitchenObject().FryingTimer;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                });
                if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                    burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    state = State.Fried;
                    burningTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                }

                break;
            case State.Fried:
                GetKitchenObject().IncrementBurningTimer(Time.deltaTime);
                burningTimer = GetKitchenObject().BurningTimer;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                });
                if (burningTimer >= burningRecipeSO.burningTimerMax)
                {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                    state = State.Burned;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = 0
                    });
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
            if(!player.HasKitchenObject()) return;
            if(!TryHandlePlate(player)) return;
            state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0f
            });
        }
        else
        {
            if (!player.HasKitchenObject()) return;
            if (!HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) return;
            player.GetKitchenObject().SetKitchenObjectParent(this);
            fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            state = State.Frying;
            fryingTimer = GetKitchenObject().FryingTimer;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
            });
        }
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
        return state == State.Fried;
    }
}
