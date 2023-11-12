using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject[] visualGameObjects;
    void Start()
    {
        Player.Instance.OnSelectedCounterChanged += ChangedCounterVisual;
    }

    private void ChangedCounterVisual(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        Show(e.selectedCounter == clearCounter);
    }

    private void Show(bool isShow)
    {
        foreach(GameObject visualGameObject in visualGameObjects)
        {
            visualGameObject.SetActive(isShow);
        }
    }
}
