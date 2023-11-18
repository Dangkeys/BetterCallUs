using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        stoveCounter.OnStateChanged += PlaySizzlingSound;
    }

    private void PlaySizzlingSound(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if(e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried)
        {
            audioSource.Play();
        }else{
            audioSource.Pause();
        }
    }
}
