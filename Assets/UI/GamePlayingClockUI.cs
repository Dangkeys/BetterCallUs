using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] Image timerImage;
    private void Update() {
       timerImage.fillAmount =  KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
