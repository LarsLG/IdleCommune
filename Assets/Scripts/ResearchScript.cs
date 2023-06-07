using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchScript : MonoBehaviour
{
    private Button closeButton;
    void Start()
    {
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(TaskOnClick);
    }
    private void TaskOnClick()
    {
        gameObject.SetActive(false);
    }
}
