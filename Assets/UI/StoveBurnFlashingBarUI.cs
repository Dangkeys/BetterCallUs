using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    const string IS_FLASHING = "IsFlashing";
    [SerializeField] StoveCounter stoveCounter;
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        stoveCounter.OnProgressChanged += (object sender, IHasProgress.OnProgressChangedEventArgs e) =>
        {
            float burnShowProgressAmount = .5f;
            bool show = e.progressNormalized >= burnShowProgressAmount && stoveCounter.IsFried();
            animator.SetBool(IS_FLASHING, show);
        };
        animator.SetBool(IS_FLASHING, false);
    }

}
