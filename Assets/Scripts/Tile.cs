using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Material baseColor, offsetColor;
    [SerializeField] private MeshRenderer tileRenderer;
    public void Init(bool isOffset)
    {
        tileRenderer.material = isOffset ? offsetColor : baseColor;
        //Debug.Log(tileRenderer.material.color);
    }
}