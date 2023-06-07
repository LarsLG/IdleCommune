using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightScript : MonoBehaviour
{
    [SerializeField] private Material baseColor, changeColor, posColor, negColor;
    [SerializeField] private MeshRenderer tileRenderer;
    public void Init(float determinfloat)
    {
        Color color = Color.Lerp(Color.red, Color.green, determinfloat);
        if (determinfloat < 0)
        {
            tileRenderer.material = baseColor;
        }
        else if (determinfloat >= 0)
        {
            changeColor.color = color;
            tileRenderer.material = changeColor;
        }
        else
        {
            tileRenderer.material = baseColor;
        }
    }
}
