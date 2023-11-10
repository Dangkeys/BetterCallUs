using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    public bool IsWalking { get; private set; }

    private void Update()
    {
        HandleMoveMent();
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
    private void HandleMoveMent()
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
}
