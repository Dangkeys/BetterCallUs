using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    const string CUT = "Cut";
    [SerializeField] CuttingCounter cuttingCounter;
    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void Start() {
        cuttingCounter.OnCut += PlayAnimation;
    }

    private void PlayAnimation(object sender, EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
