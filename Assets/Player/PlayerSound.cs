using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    Player player;
    float footstepTimer;
    float footstepTimerMax = .1f;
    void Awake()
    {
        player = GetComponent<Player>();
    }
    void Update()
    {
        footstepTimer -= Time.deltaTime;
        if(footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax;
            if(player.IsWalking)
                SoundManager.Instance.PlayFootstepSound(player.transform.position);
        }
    }
}
