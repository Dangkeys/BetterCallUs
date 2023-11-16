using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    
    [SerializeField] StoveCounter stoveCounter;
    [SerializeField] GameObject[] gameObjectVFXs;
    private void Start() {
        stoveCounter.OnStateChanged += ShowVFXs;
    }

    private void ShowVFXs(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        foreach(GameObject gameObject in gameObjectVFXs)
        {
            gameObject.SetActive(showVisual);
        }
    }
}
