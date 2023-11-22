using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private const string IS_WALKING  = "IsWalking";
    [SerializeField] Player player;
    Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void Update() {
        if(!IsOwner) return;
        animator.SetBool(IS_WALKING, player.IsWalking);
    }
}
