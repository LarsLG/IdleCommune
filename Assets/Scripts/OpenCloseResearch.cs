using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCloseResearch : MonoBehaviour
{
    [SerializeField] private GameObject ResearchPanel;
    public void SwitchResearch()
    {
        bool panelcheck = ResearchPanel.activeSelf;
        if (panelcheck)
        {
            ResearchPanel.SetActive(false);
        }
        else
        {
            ResearchPanel.SetActive(true);
        }
    }
}
