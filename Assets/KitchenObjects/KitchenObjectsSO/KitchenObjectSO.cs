using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class KitchenObjectSO : ScriptableObject
{
    [Header("References")]
    [SerializeField] private  Transform prefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string objectName;
    public Transform GetPrefabTransform()
    {
        return prefab;
    }
    public Sprite GetSprite()
    {
        return sprite;
    }
    public string GetObjectName()
    {
        return objectName;
    }
}
