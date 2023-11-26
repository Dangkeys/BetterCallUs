using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    AudioSource audioSource;
    float warningSoundTimer;
    bool playWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        stoveCounter.OnStateChanged += PlaySizzlingSound;
        stoveCounter.OnProgressChanged += (object sender, IHasProgress.OnProgressChangedEventArgs e) =>
        {
            float burnShowProgressAmount = .5f;
            playWarningSound = e.progressNormalized >= burnShowProgressAmount && stoveCounter.IsFried();
        };

    }
    void Update()
    {
        if (playWarningSound)
        {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0f)
            {
                float warningSoundTimerMax = .2f;
                warningSoundTimer = warningSoundTimerMax;
                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
        audioSource.volume = SoundManager.Instance.GetVolume();
    }
    private void PlaySizzlingSound(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if (e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}
