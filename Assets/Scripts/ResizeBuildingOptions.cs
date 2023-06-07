using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeBuildingOptions : MonoBehaviour
{
    private VerticalLayoutGroup verticalLayout;
    private RectTransform rectTransform;
    private int childCount;
    private void Awake()
    {
        verticalLayout = GetComponent<VerticalLayoutGroup>();
        rectTransform = verticalLayout.GetComponent<RectTransform>();
    }
    private void Start()
    {
        ResizeBuildOptions();
    }
    private void Update()
    {
        if (activeChildren() != childCount)
        {
            ResizeBuildOptions();
        }
    }
    public void ResizeBuildOptions()
    {
        childCount = activeChildren();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, childCount * 220);
    }
    public int activeChildren()
    {
        int activeChildCount = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                activeChildCount++;
            }
        }
        return activeChildCount;
    }
}
