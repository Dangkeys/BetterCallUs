using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] Transform counterTopPoint;
    [SerializeField] Transform plateVisualPrefab;
    [SerializeField] PlatesCounter platesCounter;
    List<GameObject> platesVisualGameObject;
    private void Awake() {
        platesVisualGameObject = new List<GameObject>();
    }
    private void Start() {
        platesCounter.OnplateSpawned += SpawnDummyPlatesVisual;
        platesCounter.OnplateRemoved += RemoveDummyPlatesVisual;
    }

    private void RemoveDummyPlatesVisual(object sender, EventArgs e)
    {
        GameObject plateGameObject = platesVisualGameObject[platesVisualGameObject.Count - 1];
        platesVisualGameObject.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void SpawnDummyPlatesVisual(object sender, EventArgs e)
    {
       Transform plateVisualTransform =  Instantiate(plateVisualPrefab, counterTopPoint);

       float plateOffsetY =.1f;
       plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * platesVisualGameObject.Count, 0);
       platesVisualGameObject.Add(plateVisualTransform.gameObject);
    }
}
