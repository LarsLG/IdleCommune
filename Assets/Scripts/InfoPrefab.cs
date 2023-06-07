using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPrefab : MonoBehaviour
{
    private GameplayManager GameplayManager;
    private TextMeshProUGUI myTextMeshPro;
    private Button buildButton;
    private building building;
    private string description;
    public void InfoPrefabVariables(GameplayManager manager, building buildingc, string desc)
    {
        GameplayManager = manager;
        building = buildingc;
        description = desc;
    }
    void Start()
    {
        myTextMeshPro = transform.Find("BuildingDescription").GetComponent<TextMeshProUGUI>();
        myTextMeshPro.text = description;
        buildButton = transform.Find("RemoveButton").GetComponent<Button>();
        buildButton.onClick.AddListener(TaskOnClick);
    }
    private void TaskOnClick()
    {
        building value;
        GameplayManager.buildingData.TryGetValue(new Vector2(GameplayManager.highXcur, GameplayManager.highZcur), out value);
        if (value is building)
        {
            Debug.Log($"True:{value}");
            //Debug.Log($"{GameplayManager.highXcur},{GameplayManager.highZcur} comparisan");
            GameplayManager.buildingData[new Vector2(GameplayManager.highXcur, GameplayManager.highZcur)] = null;
            GameplayManager.buildingGrafik[new Vector3(GameplayManager.highXcur, 1, GameplayManager.highZcur)] = null;
            GameplayManager.CloseAllPanels();
        }
        else
        {
            Debug.Log($"False:{value}");
        }
    }
}
