using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] MeshRenderer headMeshRenderer;
    [SerializeField] MeshRenderer bodyMeshRenderer;
    Material material;
    private void Awake()
    {
        material = new(headMeshRenderer.material);
        headMeshRenderer.material = material;
        bodyMeshRenderer.material = material;
    }

    public void  SetPlayerColor(Color color)
    {
        material.color = color;
        headMeshRenderer.material = material;
        bodyMeshRenderer.material = material;
    }
}
