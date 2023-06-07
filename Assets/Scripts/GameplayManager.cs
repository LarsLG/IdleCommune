using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    [HideInInspector] public int highXcur, highZcur;
    [HideInInspector] public int width, height;
    [HideInInspector] public ObservableDictionary<Vector2, building> buildingData;
    [HideInInspector] public ObservableDictionary<Vector3, GameObject> buildingGrafik;
    [HideInInspector] public List<BuildingExtraData> BuildingDataList;
    [HideInInspector] public List<GameObject> buildingPanelList;
    private string PrefabBuildingsFolder = "Buildings";
    [HideInInspector] public List<GameObject> buildBuildingPanelList;
    [HideInInspector] public List<ResearchNode> PossibleResearch = new List<ResearchNode>();
    public GameObject constructionSitePrefab;
    public Canvas progressBarPrefab;
    [SerializeField] private GameObject ResourcePanel;
    private Dictionary<string, TextMeshProUGUI> RessourcePanels = new Dictionary<string, TextMeshProUGUI>();
    [SerializeField] private GameObject WorkerPanel;
    private Dictionary<string, TextMeshProUGUI> WorkerTexts = new Dictionary<string, TextMeshProUGUI>();
    private Dictionary<string, Slider> WorkerSliders = new Dictionary<string, Slider>();
    private List<string> PrefabNameList = new List<string>(); // { "Mine", "Workshop", "Farm", "Hut" };

    public int Material;
    public float MaterialProduction;
    private float fractionalMaterialProduction = 0f;
    public int Food;
    public float FoodProduction;
    private float fractionalFoodProduction = 0f;
    public float Production = 0f;
    public float Research;
    public int LivingSpace;
    public float Energy;
    public float Happiness;
    public int BuildingCount;

    private List<(float, float)> foodworker;
    private List<(float, float)> mateworker;
    private List<(float, float, float)> workworker;
    private List<(float, float, float)> reasworker;

    void Start()
    {
        //Ressource display base after calculation
        TextMeshProUGUI[] allTextComponents = ResourcePanel.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textComponent in allTextComponents)
        {
            RessourcePanels[textComponent.name] = textComponent;
            //Debug.Log(textComponent.name);
        }
        //Objects for Worker input
        TextMeshProUGUI[] allWorkerTexts = WorkerPanel.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI WorkerText in allWorkerTexts)
        {
            WorkerTexts[WorkerText.name] = WorkerText;
            //Debug.Log(WorkerText.name);
        }
        Slider[] allWorkerSliders = WorkerPanel.GetComponentsInChildren<Slider>();
        foreach (Slider WorkerSlider in allWorkerSliders)
        {
            WorkerSliders[WorkerSlider.name] = WorkerSlider;
            WorkerSlider.onValueChanged.AddListener(OnSliderValueChanged);
            WorkerSlider.onValueChanged.AddListener((value) => SliderWorkerCalc(WorkerSlider));

            WorkerSlider.interactable = false;
            //Debug.Log(WorkerSlider.name);
        }
        //collection for deletion
        GameObject[] buildingPrefabList = Resources.LoadAll<GameObject>(PrefabBuildingsFolder);
        foreach (GameObject prefab in buildingPrefabList)
        {
            PrefabNameList.Add(prefab.name);
        }
    }
    void Update()
    {
        fractionalFoodProduction += FoodProduction * Time.deltaTime / 60; // Add the production rate * time to the fractional part
        if (fractionalFoodProduction >= 1f) // If the fractional part is 1 or greater
        {
            int numToAdd = (int)fractionalFoodProduction; // Calculate how many full objects to add
            Food += numToAdd; // Add the full objects to the integer value
            fractionalFoodProduction -= numToAdd; // Subtract the added objects from the fractional part
        }
        else if (fractionalFoodProduction <= -1f)
        {
            int numToAdd = (int)fractionalFoodProduction; // Calculate how many full objects to add
            Food += numToAdd; // Add the full objects to the integer value
            fractionalFoodProduction -= numToAdd;
        }
        fractionalMaterialProduction += MaterialProduction * Time.deltaTime / 60; // Add the production rate * time to the fractional part
        if (fractionalMaterialProduction >= 1f) // If the fractional part is 1 or greater
        {
            int numToAdd = (int)fractionalMaterialProduction; // Calculate how many full objects to add
            Material += numToAdd; // Add the full objects to the integer value
            fractionalMaterialProduction -= numToAdd; // Subtract the added objects from the fractional part
        }
        else if (fractionalMaterialProduction <= -1f)
        {
            int numToAdd = (int)fractionalMaterialProduction; // Calculate how many full objects to add
            Material += numToAdd; // Add the full objects to the integer value
            fractionalMaterialProduction -= numToAdd;
        }
        if (RessourcePanels.TryGetValue("FoodStorage", out TextMeshProUGUI FoodStorage)) { FoodStorage.text = $"{Food}"; }
        if (RessourcePanels.TryGetValue("MaterialStorage", out TextMeshProUGUI MaterialStorage)) { MaterialStorage.text = $"{Material}"; }
    }
    public void renderBuildings()
    {
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            if (PrefabNameList.Contains(obj.name))
            {
                Destroy(obj);
            }
        }
        foreach (KeyValuePair<Vector3, GameObject> pair in buildingGrafik)
        {
            if (pair.Value != null)
            {
                var spawnedBuilding = Instantiate(pair.Value, pair.Key, pair.Value.transform.rotation);
                spawnedBuilding.name = $"{pair.Value.name}";
            }
        }
    }
    public void calculateOutput()
    {
        MaterialProduction = 0;
        FoodProduction = 0;
        Production = 0;
        Research = 0;
        LivingSpace = 0;
        Energy = 0;
        Happiness = 0;
        BuildingCount = 0;
        foodworker = new List<(float, float)>();
        mateworker = new List<(float, float)>();
        workworker = new List<(float, float, float)>();
        reasworker = new List<(float, float, float)>();
        foreach (KeyValuePair<Vector2, building> pair in buildingData)
        {
            if (pair.Value is not UnlockTile && pair.Value != null)
            {
                //Creat base Building clone and overwirte current stats
                string CurrName = pair.Value.name;
                //Debug.Log(CurrName);
                building childBuilding = (building)Activator.CreateInstance(Type.GetType(CurrName));
                Debug.Log(childBuilding);
                foreach (var property in pair.Value.GetType().GetProperties())
                {
                    //Debug.Log(property.Name);
                    PropertyInfo OverwriteAttribut = pair.Value.GetType().GetProperty(property.Name);
                    var changeValue = OverwriteAttribut.GetValue(childBuilding);
                    OverwriteAttribut.SetValue(pair.Value, changeValue);
                }
                foreach (Vector2 pos in pair.Value.EffectAera)
                {
                    Vector2 posEffect = pair.Key + pos;
                    //Debug.Log(posEffect);
                    if (buildingData.TryGetValue(posEffect, out building effector))
                    {
                        if (effector is not UnlockTile && effector != null)
                        {
                            //Debug.Log(effector);
                            foreach (MyTuple<Tags, GVM, float> item in effector.RessourceEffect)
                            {
                                if (pair.Value.BuildingTags.Contains(item.I1))
                                {
                                    //Debug.Log(item.I2.ToString());
                                    //string CurrName = pair.Value.GetType().GetProperty("name").GetValue(pair.Value).ToString();
                                    PropertyInfo CurrAttribut = pair.Value.GetType().GetProperty(item.I2.ToString());
                                    //Debug.Log(CurrAttribut.Name);
                                    //Debug.Log(CurrAttribut.GetValue(pair.Value));
                                    float changeValue = (float)CurrAttribut.GetValue(pair.Value) * item.I3;
                                    //Debug.Log(changeValue);
                                    CurrAttribut.SetValue(pair.Value, changeValue);
                                }
                            }
                        }
                    }
                }


                //Worker lists for values and calculation
                if (pair.Value.FoodWorkerProd > 0)
                {
                    for (int i = 0; i < pair.Value.MaxWorker; i++) { foodworker.Add((pair.Value.FoodWorkerProd, pair.Value.WorkerMaterialUse)); }
                }
                if (pair.Value.MaterialWorkerProd > 0)
                {
                    for (int i = 0; i < pair.Value.MaxWorker; i++) { mateworker.Add((pair.Value.MaterialWorkerProd, pair.Value.WorkerFoodUse)); }
                }
                if (pair.Value.WorkWorkerProd > 0)
                {
                    for (int i = 0; i < pair.Value.MaxWorker; i++) { workworker.Add((pair.Value.WorkWorkerProd, pair.Value.WorkerFoodUse, pair.Value.WorkerMaterialUse)); }
                }
                if (pair.Value.ResearchWorkerProd > 0)
                {
                    for (int i = 0; i < pair.Value.MaxWorker; i++) { reasworker.Add((pair.Value.ResearchWorkerProd, pair.Value.WorkerFoodUse, pair.Value.WorkerMaterialUse)); }
                }
                //Base Production Values
                MaterialProduction += pair.Value.MaterialBaseProd;
                MaterialProduction -= pair.Value.MaterialUse;
                FoodProduction += pair.Value.FoodBaseProd;
                FoodProduction -= pair.Value.FoodUse;
                Production += pair.Value.WorkBaseProd;
                Research += pair.Value.ResearchBaseProd;
                LivingSpace += pair.Value.LivingSpace;
                Energy += pair.Value.EnergyUse;
                Happiness += pair.Value.Happiness;
                BuildingCount += 1;
            }
        }
        //Sort Lists with values
        foodworker.Sort((x, y) => y.Item1.CompareTo(x.Item1));
        mateworker.Sort((x, y) => y.Item1.CompareTo(x.Item1));
        workworker.Sort((x, y) => y.Item1.CompareTo(x.Item1));
        reasworker.Sort((x, y) => y.Item1.CompareTo(x.Item1));
        //Worker Slider Output
        if (WorkerSliders.TryGetValue("FoodSlider", out Slider FoodSlider))
        {
            if (foodworker.Count == 0)
            {
                FoodSlider.interactable = false;
                FoodSlider.value = 0;
            }
            else
            {
                FoodSlider.interactable = true;
                FoodSlider.maxValue = foodworker.Count;
                FoodSlider.minValue = 0;
            }
            //SliderDescriptionChange();
        }
        if (WorkerSliders.TryGetValue("MaterialSlider", out Slider MaterialSlider))
        {
            if (mateworker.Count == 0)
            {
                MaterialSlider.interactable = false;
                MaterialSlider.value = 0;
            }
            else
            {
                MaterialSlider.interactable = true;
                MaterialSlider.maxValue = mateworker.Count;
                MaterialSlider.minValue = 0;
            }
            //SliderDescriptionChange();
        }
        if (WorkerSliders.TryGetValue("WorkSlider", out Slider WorkSlider))
        {
            if (workworker.Count == 0)
            {
                WorkSlider.interactable = false;
                WorkSlider.value = 0;
            }
            else
            {
                WorkSlider.interactable = true;
                WorkSlider.maxValue = workworker.Count;
                WorkSlider.minValue = 0;
            }
            //SliderDescriptionChange();
        }
        if (WorkerSliders.TryGetValue("ResearchSlider", out Slider ResearchSlider))
        {
            if (reasworker.Count == 0)
            {
                ResearchSlider.interactable = false;
                ResearchSlider.value = 0;
            }
            else
            {
                ResearchSlider.interactable = true;
                ResearchSlider.maxValue = reasworker.Count;
                ResearchSlider.minValue = 0;
            }
            //SliderDescriptionChange();
        }
        if (WorkerTexts.TryGetValue("FoodMax", out TextMeshProUGUI FoodMax))
        {
            if (foodworker.Count == 0)
            {
                FoodMax.text = "0/0";
            }
            else
            {
                FoodMax.text = $"{FoodSlider.value}/{foodworker.Count}";
            }
        }
        if (WorkerTexts.TryGetValue("MaterialMax", out TextMeshProUGUI MaterialMax))
        {
            if (mateworker.Count == 0)
            {
                MaterialMax.text = "0/0";
            }
            else
            {
                MaterialMax.text = $"{MaterialSlider.value}/{mateworker.Count}";
            }
        }
        if (WorkerTexts.TryGetValue("WorkMax", out TextMeshProUGUI WorkMax))
        {
            if (workworker.Count == 0)
            {
                WorkMax.text = "0/0";
            }
            else
            {
                WorkMax.text = $"{WorkSlider.value}/{workworker.Count}";
            }
        }
        if (WorkerTexts.TryGetValue("ResaerchMax", out TextMeshProUGUI ResaerchMax))
        {
            if (reasworker.Count == 0)
            {
                ResaerchMax.text = "0/0";
            }
            else
            {
                ResaerchMax.text = $"{ResearchSlider.value}/{reasworker.Count}";
            }
        }
        //add 0 after slider max readout
        foodworker.Insert(0, (0, 0));
        mateworker.Insert(0, (0, 0));
        workworker.Insert(0, (0, 0, 0));
        reasworker.Insert(0, (0, 0, 0));
        //Worker Value calculation
        if (foodworker.Count > 1)
        {
            for (int i = 0; i <= (int)FoodSlider.value && i < foodworker.Count; i++)
            {
                FoodProduction += foodworker[i].Item1;
                MaterialProduction -= foodworker[i].Item2;
            }
        }
        if (mateworker.Count > 1)
        {
            for (int i = 0; i <= (int)MaterialSlider.value && i < mateworker.Count; i++)
            {
                MaterialProduction += mateworker[i].Item1;
                FoodProduction -= mateworker[i].Item2;
            }
        }
        if (workworker.Count > 1)
        {
            for (int i = 0; i <= (int)WorkSlider.value && i < workworker.Count; i++)
            {
                Production += workworker[i].Item1;
                FoodProduction -= workworker[i].Item2;
                MaterialProduction -= workworker[i].Item3;
            }
        }
        if (reasworker.Count > 1)
        {
            for (int i = 0; i <= (int)ResearchSlider.value && i < reasworker.Count; i++)
            {
                Research += reasworker[i].Item1;
                FoodProduction -= reasworker[i].Item2;
                MaterialProduction -= reasworker[i].Item3;
            }
        }
        //Ressources Output
        if (RessourcePanels.TryGetValue("Food", out TextMeshProUGUI FoodText)) { FoodText.text = $"{FoodProduction.ToString("0.00")}"; }
        if (RessourcePanels.TryGetValue("Material", out TextMeshProUGUI MaterialText)) { MaterialText.text = $"{MaterialProduction.ToString("0.00")}"; }
        if (RessourcePanels.TryGetValue("Production", out TextMeshProUGUI ProductionText)) { ProductionText.text = $"{Production.ToString("0.00")}"; }
        if (RessourcePanels.TryGetValue("Research", out TextMeshProUGUI ResearchText)) { ResearchText.text = $"{Research.ToString("0.00")}"; }
        if (RessourcePanels.TryGetValue("LivingSpace", out TextMeshProUGUI LivingSpaceText)) { LivingSpaceText.text = $"{LivingSpace}"; }
        if (RessourcePanels.TryGetValue("Energy", out TextMeshProUGUI EnergyText)) { EnergyText.text = $"{Energy.ToString("0.0")}"; }
        if (RessourcePanels.TryGetValue("Happiness", out TextMeshProUGUI HappinessText)) { HappinessText.text = $"{Happiness.ToString("0.0")}"; }
    }
    private void OnSliderValueChanged(float value)
    {
        //Slider slider = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
        //Debug.Log(slider);
        calculateOutput();
        SliderDescriptionChange();
    }
    private void SliderDescriptionChange()
    {
        if (WorkerTexts.TryGetValue("SliderFood", out TextMeshProUGUI SliderFood))
        {
            if (foodworker.Count == 0)
            {
                SliderFood.text = $"Food: +0";
            }
            else
            {
                SliderFood.text = $"Food: <color=green>+{foodworker[(int)WorkerSliders["FoodSlider"].value].Item1}</color>";
            }
        }
        if (WorkerTexts.TryGetValue("SliderMaterial", out TextMeshProUGUI SliderMaterial))
        {
            if (mateworker.Count == 0)
            {
                SliderMaterial.text = $"Material: +0";
            }
            else
            {
                SliderMaterial.text = $"Material: <color=green>+{mateworker[(int)WorkerSliders["MaterialSlider"].value].Item1}</color>";
            }
        }
        if (WorkerTexts.TryGetValue("SliderWork", out TextMeshProUGUI SliderWork))
        {
            if (workworker.Count == 0)
            {
                SliderWork.text = $"Production: +0";
            }
            else
            {
                SliderWork.text = $"Production: <color=green>+{workworker[(int)WorkerSliders["WorkSlider"].value].Item1}</color>";
            }
        }
        if (WorkerTexts.TryGetValue("SliderResearch", out TextMeshProUGUI SliderResearch))
        {
            if (reasworker.Count == 0)
            {
                SliderResearch.text = $"Research: +0";
            }
            else
            {
                SliderResearch.text = $"Research: <color=green>+{reasworker[(int)WorkerSliders["ResearchSlider"].value].Item1}</color>";
            }
        }
    }
    //private void SliderDescriptionChange(float value, Slider slider)
    //{
    //    if (WorkerTexts.TryGetValue("SliderFood", out TextMeshProUGUI SliderFood) && slider.name == "FoodSlider")
    //    {
    //        if (foodworker.Count == 0)
    //        {
    //            SliderFood.text = $"Food: +0";
    //        }
    //        else
    //        {
    //            SliderFood.text = $"Food: <color=green>+{foodworker[(int)value]}</color>";
    //        }
    //    }
    //    if (WorkerTexts.TryGetValue("SliderMaterial", out TextMeshProUGUI SliderMaterial) && slider.name == "MaterialSlider")
    //    {
    //        if (mateworker.Count == 0)
    //        {
    //            SliderMaterial.text = $"Material: +0";
    //        }
    //        else
    //        {
    //            SliderMaterial.text = $"Material: <color=green>+{mateworker[(int)value]}</color>";
    //        }
    //    }
    //    if (WorkerTexts.TryGetValue("SliderWork", out TextMeshProUGUI SliderWork) && slider.name == "WorkSlider")
    //    {
    //        if (workworker.Count == 0)
    //        {
    //            SliderWork.text = $"Production: +0";
    //        }
    //        else
    //        {
    //            SliderWork.text = $"Production: <color=green>+{workworker[(int)value]}</color>";
    //        }
    //    }
    //    if (WorkerTexts.TryGetValue("SliderResearch", out TextMeshProUGUI SliderResearch) && slider.name == "ResearchSlider")
    //    {
    //        if (reasworker.Count == 0)
    //        {
    //            SliderResearch.text = $"Research: +0";
    //        }
    //        else
    //        {
    //            SliderResearch.text = $"Research: <color=green>+{reasworker[(int)value]}</color>";
    //        }
    //    }
    //}
    private void SliderWorkerCalc(Slider CurrentSlider)
    {
        Dictionary<string, Slider> copyWorkerSliders = new Dictionary<string, Slider>(WorkerSliders);
        //Slider CurrentSlider = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
        //Debug.Log(slider);
        copyWorkerSliders.Remove(CurrentSlider.name);
        //Debug.Log(copyWorkerSliders.Count);

        float currentSum = WorkerSliders.Sum(s => s.Value.value);
        Debug.Log(currentSum);
        if (currentSum > LivingSpace)
        {
            Slider highestSlider = copyWorkerSliders.Values
                //.Where(s => s != slider)
                .OrderByDescending(s => s.value)
                .First();
            highestSlider.value -= 1;
        }
        if (CurrentSlider.value > LivingSpace)
        {
            CurrentSlider.value -= 1;
        }
    }
    public void CloseAllPanels()
    {
        foreach (GameObject gameObject in buildingPanelList)
        {
            gameObject.SetActive(false); // deaktivate all Buildinginfos
        }
        NotifyAllPanlesClosed();
    }
    //Event and Action when all Panels are closed
    public delegate void OnCloseAllPanelsDelegate();

    public event OnCloseAllPanelsDelegate OnCloseAllPanels;
    private void NotifyAllPanlesClosed()
    {
        if (OnCloseAllPanels != null)
        {
            OnCloseAllPanels();
        }
    }
    public static int MaterialGrowth(int a, int x)
    {
        int y = (int)Math.Ceiling(a * Math.Pow(1.2d, x));
        return y;
    }
    //buildPanel Function to contruct Buildings
    public void BuildingCocunt(int productionNeeded, building toBuild, GameObject buildingPrefab, int curX, int curZ)
    {
        StartCoroutine(IncreaseGrowth(productionNeeded, toBuild, buildingPrefab, curX, curZ));
    }
    public IEnumerator IncreaseGrowth(int productionNeeded, building toBuild, GameObject buildingPrefab, int curX, int curZ)
    {
        var spawnedCanvas = Instantiate(progressBarPrefab, new Vector3(curX, 1, curZ), Quaternion.identity);
        Slider spawnedProgressBar = spawnedCanvas.GetComponentInChildren<Slider>();
        spawnedProgressBar.maxValue = productionNeeded;
        float buildingState = 0.0f;
        //Debug.Log(productionNeeded);
        //Debug.Log(production);
        while (buildingState <= productionNeeded)
        {
            int productionBuildingsNeeded = buildingData.Values.Count(v => v is constructionSite);
            if (productionBuildingsNeeded <= 0)
            {
                productionBuildingsNeeded = 1;
            }
            //Debug.Log($"Number of current constructions: {productionBuildingsNeeded}");
            buildingState += Production / productionBuildingsNeeded * Time.deltaTime;
            spawnedProgressBar.value = buildingState;
            //Debug.Log(buildingState);
            yield return null;
        }
        buildingData[new Vector2(curX, curZ)] = toBuild;
        buildingGrafik[new Vector3(curX, 1, curZ)] = buildingPrefab;
        Destroy(spawnedCanvas);
    }
    public void ResearchChecks()
    {
        ResearchActivityState();
        ResearchDone();
    }
    public void ResearchDone()
    {
        foreach (ResearchNode Node in PossibleResearch)
        {
            Node.CompletionState();
            if (Node.IsCompleted)
            {
                Node.ResearchItem.SetActive(true);
                Node.ResearchDisplay.SetActive(false);
                //Debug.Log(Node.ResearchItem.name);
            }
            else
            {
                Node.ResearchItem.SetActive(false);
            }
        }
    }
    public void ResearchActivityState()
    {
        foreach (ResearchNode Node in PossibleResearch)
        {
            Node.ActivityState();
            if (Node.IsActive)
            {
                Node.ResearchDisplay.SetActive(true);
                //Debug.Log(Node.ResearchItem.name);
            }
            else
            {
                Node.ResearchDisplay.SetActive(false);
            }
        }
    }
    public int ResearchCompleted()
    {
        int completedResearch = 0;
        foreach (ResearchNode Node in PossibleResearch)
        {
            if (Node.IsCompleted)
            {
                completedResearch += 1;
            }
        }
        return completedResearch;
    }
    public void Researching(ResearchNode Node, GameObject researchOptions)
    {
        StartCoroutine(IncreaseResearch(Node, researchOptions));
    }
    public IEnumerator IncreaseResearch(ResearchNode Node, GameObject researchOptions)
    {
        Debug.Log("research started");
        Slider ProgressBar = Node.ResearchDisplay.GetComponent<ResearchPrefab>().progressbar;
        int researchNeeded = MaterialGrowth(Node.ResearchNeeded,ResearchCompleted());
        ProgressBar.maxValue = researchNeeded;
        float researchState = 0.0f;
        while (researchState <= researchNeeded)
        {
            researchState += Research * Time.deltaTime;
            ProgressBar.value = researchState;
            //Debug.Log(researchState);
            yield return null;
        }
        Node.IsCompleted = true;
        ResearchChecks();
        researchOptions.GetComponent<ResizeBuildingOptions>().ResizeBuildOptions();
    }
    public void UnlockTileLoad(int productionNeeded, int curX, int curZ)
    {
        StartCoroutine(UnlockGrowth(productionNeeded, curX, curZ));
    }
    public IEnumerator UnlockGrowth(int productionNeeded, int curX, int curZ)
    {
        var spawnedCanvas = Instantiate(progressBarPrefab, new Vector3(curX, 1, curZ), Quaternion.identity);
        Slider spawnedProgressBar = spawnedCanvas.GetComponentInChildren<Slider>();
        GameObject unlocktileinfo = buildingPanelList.FirstOrDefault(obj => obj.name == "UnlockTile(Info)");
        spawnedProgressBar.maxValue = productionNeeded;
        float buildingState = 0.0f;
        while (buildingState <= productionNeeded)
        {
            buildingState += Production * Time.deltaTime;
            spawnedProgressBar.value = buildingState;
            if (unlocktileinfo.activeSelf)
            {
                unlocktileinfo.SetActive(false);
            }
            yield return null;
        }
        buildingData[new Vector2(curX, curZ)] = null;
        buildingGrafik[new Vector3(curX, 1, curZ)] = null;
        Destroy(spawnedCanvas);
    }
}
