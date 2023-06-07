using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BuildBuidlingPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameplayManager GameplayManager;
    private GameObject buildingPrefab;
    private GameObject MainCamera;
    private TextMeshProUGUI myTextMeshPro;
    private Button buildButton;
    private building thebuilding;
    public void BuildBuidlingPrefabVariables(GameplayManager manager, GameObject prefab, GameObject camera, building buildingc)
    {
        GameplayManager = manager;
        buildingPrefab = prefab;
        MainCamera = camera;
        thebuilding = buildingc;
    }
    void Start()
    {
        myTextMeshPro = transform.Find("BuildingDescription").GetComponent<TextMeshProUGUI>();
        buildButton = transform.Find("Button").GetComponent<Button>();
        buildButton.onClick.AddListener(TaskOnClick);
        textUpdate();
        GameplayManager.OnCloseAllPanels += textUpdate;
    }
    public virtual void textUpdate()
    {
        string description = "";
        // Get all public fields of the class Building
        PropertyInfo[] properties = thebuilding.GetType().GetProperties();
        // Iterate over the fields and check their types
        foreach (PropertyInfo property in properties)
        {
            object fieldValue = property.GetValue(thebuilding);
            Type fieldType = fieldValue.GetType();

            //Debug.Log($"Field Name: {property.Name}, Field Type: {fieldType}, Field Value: {fieldValue}");
            // Check the type of the field
            if (fieldType == typeof(string))
            {
                description += $"Build {fieldValue}:\n";
            }
            else if (fieldType == typeof(int))
            {
                if (property.Name == "matrials")
                {
                    int matrials = GameplayManager.MaterialGrowth((int)fieldValue, GameplayManager.BuildingCount);
                    description += $"Material Cost: {matrials} \t";
                }
                else if (property.Name == "production")
                {
                    int production = GameplayManager.MaterialGrowth((int)fieldValue, GameplayManager.BuildingCount);
                    description += $"Work Needed: {production} \n";
                }
                else
                {
                    if ((int)fieldValue != 0)
                    {
                        description += $"{property.Name}: {fieldValue} ";
                    } 
                }
            }
            else if (fieldType == typeof(float))
            {
                if ((float)fieldValue != 0)
                {
                    description += $"{property.Name}: {fieldValue} ";
                }
            }
        }
        myTextMeshPro.text = description;
    }
    private void TaskOnClick()
    {
        building value;
        int matrials = GameplayManager.MaterialGrowth(thebuilding.matrials, GameplayManager.BuildingCount);
        int production = GameplayManager.MaterialGrowth(thebuilding.production, GameplayManager.BuildingCount);
        GameplayManager.buildingData.TryGetValue(new Vector2(GameplayManager.highXcur, GameplayManager.highZcur), out value);
        if (value == null && GameplayManager.Material >= matrials)
        {
            GameplayManager.Material -= matrials;
            GameplayManager.CloseAllPanels();
            Debug.Log("closed all panels");
            var SourceHighlightScript = MainCamera.GetComponent<MouseRayCast>();
            SourceHighlightScript.DestroyAllHighlights();
            SourceHighlightScript.ToggleHighlight(GameplayManager.highXcur, GameplayManager.highZcur);
            Debug.Log($"True:{value}");
            if (production == 0)
            {
                GameplayManager.buildingData[new Vector2(GameplayManager.highXcur, GameplayManager.highZcur)] = thebuilding;
                GameplayManager.buildingGrafik[new Vector3(GameplayManager.highXcur, 1, GameplayManager.highZcur)] = buildingPrefab;
            }
            else
            {
                GameplayManager.buildingData[new Vector2(GameplayManager.highXcur, GameplayManager.highZcur)] = new constructionSite(buildingPrefab, thebuilding, GameplayManager, GameplayManager.highXcur, GameplayManager.highZcur);
                GameplayManager.buildingGrafik[new Vector3(GameplayManager.highXcur, 1, GameplayManager.highZcur)] = GameplayManager.constructionSitePrefab;
            }

        }
        else
        {
            Debug.Log($"False:{value}");
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        var SourceHighlightScript = MainCamera.GetComponent<MouseRayCast>();
        SourceHighlightScript.DestroyAllHighlights();
        foreach (Vector2 pos in thebuilding.EffectAera)
        {
            Vector2 posEffect = new Vector2(GameplayManager.highXcur, GameplayManager.highZcur) + pos;
            //Check exsistens
            float posint = 0;
            float negint = 0;
            int countint = 0;
            if (GameplayManager.buildingData.TryGetValue(posEffect, out building effector))
            {
                if (effector != null)
                {
                    //check Tags
                    foreach (MyTuple<Tags, GVM, float> item in thebuilding.RessourceEffect)
                    {
                        if (effector.BuildingTags.Contains(item.I1))
                        {
                            //calulate effect
                            if (item.I2 == GVM.MaterialBaseProd ||
                                item.I2 == GVM.FoodBaseProd ||
                                item.I2 == GVM.WorkBaseProd ||
                                item.I2 == GVM.ResearchBaseProd ||
                                item.I2 == GVM.MaterialWorkerProd ||
                                item.I2 == GVM.FoodWorkerProd ||
                                item.I2 == GVM.WorkWorkerProd ||
                                item.I2 == GVM.ResearchWorkerProd)
                            {
                                if (item.I3 > 1)
                                {
                                    posint += 1;
                                }
                                else if (item.I3 < 1)
                                {
                                    negint += 1;
                                }
                                countint += 1;
                            }
                            else if (item.I2 == GVM.MaterialUse ||
                                    item.I2 == GVM.WorkerMaterialUse ||
                                    item.I2 == GVM.EnergyUse ||
                                    item.I2 == GVM.FoodUse ||
                                    item.I2 == GVM.WorkerFoodUse)
                            {
                                if (item.I3 > 1)
                                {
                                    negint += 1;
                                }
                                else if (item.I3 < 1)
                                {
                                    posint += 1;
                                }
                                countint += 1;
                            }
                        }
                    }
                }
            }
            float detim = -1f;
            if (countint != 0)
            {
                detim = posint / (negint + countint);
            }
            SourceHighlightScript.HighlightEffct((int)posEffect.x, (int)posEffect.y, detim);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        var SourceHighlightScript = MainCamera.GetComponent<MouseRayCast>();
        SourceHighlightScript.DestroyAllHighlights();
        SourceHighlightScript.ToggleHighlight(GameplayManager.highXcur, GameplayManager.highZcur);
    }
}
