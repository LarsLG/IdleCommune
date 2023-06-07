using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResearchPrefab : MonoBehaviour
{
    private GameplayManager GameplayManager;
    private TextMeshProUGUI myTextMeshPro;
    private Button buildButton;
    [HideInInspector] public Slider progressbar;
    private building building;
    private ResearchNode theNode;
    private GameObject researchOptions;
    public void ResearchPrefabVariables(GameplayManager manager, building buildingc, ResearchNode theNodec)
    {
        GameplayManager = manager;
        building = buildingc;
        theNode = theNodec;
    }
    void Start()
    {
        myTextMeshPro = transform.Find("BuildingDescription").GetComponent<TextMeshProUGUI>();
        buildButton = transform.Find("Button").GetComponent<Button>();
        buildButton.onClick.AddListener(TaskOnClick);
        progressbar = transform.Find("Slider").GetComponent<Slider>();
        textUpdate();
        GameplayManager.OnCloseAllPanels += textUpdate;
        Transform parentTransform = transform.parent;
        researchOptions = parentTransform.gameObject;
    }
    public virtual void textUpdate()
    {
        int researchCompleted = GameplayManager.ResearchCompleted();
        int research = GameplayManager.MaterialGrowth(theNode.ResearchNeeded, researchCompleted);
        string description = $"{theNode.ResearchName}:\n Research Needed: {research}";
        myTextMeshPro.text = description;
    }
    private void TaskOnClick()
    {
        GameplayManager.Researching(theNode, researchOptions);
        buildButton.interactable = false;
    }
}
