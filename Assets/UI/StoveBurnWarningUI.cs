using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    void Start()
    {
        stoveCounter.OnProgressChanged += (object sender, IHasProgress.OnProgressChangedEventArgs e) =>
        {
            float burnShowProgressAmount = .5f;
            bool show = e.progressNormalized >= burnShowProgressAmount && stoveCounter.IsFried();
            if (show)
            {
                Show();
            }
            else
            {
                Hide();
            }
        };
        Hide();
    }
    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
