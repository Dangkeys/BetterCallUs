using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public event EventHandler OnPickSomething;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;
    public static Player Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Error meow meow");
        }
    }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    public bool IsWalking { get; private set; }
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;

    private void Start()
    {
        gameInput.OnInteractAction += GameInputOnInteractAction;
        gameInput.OnInteractAlternateAction += GameInputOnInteractAlternateAction;
    }

    private void GameInputOnInteractAlternateAction(object sender, EventArgs e)
    {
        if(!KitchenGameManager.Instance.IsGamePlaying()) return;
        if(selectedCounter == null) return;
        selectedCounter.InteractAlternate(this);
    }

    private void GameInputOnInteractAction(object sender, System.EventArgs e)
    {
        if(!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter == null) return;
        selectedCounter.Interact(this);
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y).normalized;
        float interactDistance = 2f;
        if (moveDir != Vector3.zero)
            lastInteractDir = moveDir;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (selectedCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }

        }
        else
        {
            SetSelectedCounter(null);
        }
        // Debug.Log(selectedCounter);
    }
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y).normalized;

        float playerRadius = 0.7f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = TryMove(transform.position, moveDir, playerRadius, playerHeight, moveDistance);

        if (canMove)
            transform.position += moveDir * moveDistance;
        else
            TryMoveInOtherDirections(moveDir, playerRadius, playerHeight, moveDistance);

        IsWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
    private bool TryMove(Vector3 position, Vector3 direction, float radius, float height, float distance)
    {
        bool hit = Physics.CapsuleCast(position, position + Vector3.up * height, radius, direction, out RaycastHit hitInfo, distance);
        return !hit;
    }

    private void TryMoveInOtherDirections(Vector3 moveDir, float playerRadius, float playerHeight, float moveDistance)
    {
        Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
        if (TryMove(transform.position, moveDirX, playerRadius, playerHeight, moveDistance))
        {
            transform.position += moveDirX * moveDistance;
            return;
        }

        Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
        if (TryMove(transform.position, moveDirZ, playerRadius, playerHeight, moveDistance))
            transform.position += moveDirZ * moveDistance;

    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            OnPickSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
