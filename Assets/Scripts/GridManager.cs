using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameplayManager GameplayManager;
    public int width, height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private GameObject unlockTilePrefab;
    [SerializeField] private Transform maincam;
    [SerializeField] private Transform camCenter;
    private Dictionary<Vector2, Tile> tiles;
    private ObservableDictionary<Vector2, building> buildingData;
    private ObservableDictionary<Vector3, GameObject> buildingGrafik;
    [SerializeField] private GameObject InfoPrefab;
    [SerializeField] private GameObject ResearchPrefab;
    [SerializeField] private GameObject BuildBuidlingPrefab;
    [SerializeField] private GameObject SidePanel;
    [SerializeField] private GameObject BuildPanel;
    [SerializeField] private GameObject ResearchPanel;
    private string PrefabBuildingsFolder = "Buildings";
    private List<GameObject> PrefabBuildingList = new List<GameObject>();
    private List<BuildingExtraData> BuildingDataList = new List<BuildingExtraData>();

    void Start()
    {
        GameplayManager.width = width-1;
        GameplayManager.height = height-1;
        GenerateGrid();
        GenerateMapData();
        GenerateMapGrafiks();
        GenerateUnlockTiles();
        GameplayManager.buildingData = buildingData;
        GameplayManager.buildingGrafik = buildingGrafik;
        GameplayManager.renderBuildings();
        GameObject[] buildingPrefabList = Resources.LoadAll<GameObject>(PrefabBuildingsFolder);
        foreach (GameObject prefab in buildingPrefabList)
        {
            //Debug.Log(prefab);
            PrefabBuildingList.Add(prefab);
        }
        GenerateBuildingData();
        //Info Panel Creation
        CreateInfoPanels();
        //Building Panel Creation
        CreateBuildingPanels();
        //Research Panel Creation
        CreateResearchPanels();
        GameplayManager.ResearchChecks();
        GameplayManager.BuildingDataList = BuildingDataList;
    }

    void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {z}";

                var isOffset = (x % 2 == 0 && z % 2 != 0) || (x % 2 != 0 && z % 2 == 0);
                spawnedTile.Init(isOffset);

                tiles[new Vector2(x, z)] = spawnedTile;
            }
        }

        maincam.transform.position = new Vector3((float)width / 2 - 0.5f, MathF.Max(width, height), MathF.Max(width, height) * -0.5f);
        maincam.transform.rotation = Quaternion.Euler(45,0,0);
        camCenter.transform.position = new Vector3((float)width / 2 - 0.5f, (float)-1f, (float)height / 2 - 0.5f);
        maincam.transform.parent = camCenter.transform;
    }
    private void GenerateMapData()
    {
        buildingData = new ObservableDictionary<Vector2, building>();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                buildingData[new Vector2(x, z)] = null;
            }
        }
        //buildingData.OnDictionaryChanged += GameplayManager.calculateOutput;
    }
    private void GenerateMapGrafiks()
    {
        buildingGrafik = new ObservableDictionary<Vector3, GameObject>();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                buildingGrafik[new Vector3(x, 1, z)] = null;
            }
        }
        //buildingGrafik.OnDictionaryChanged += GameplayManager.renderBuildings;
    }
    private void GenerateUnlockTiles()
    {
        string buildingPrefabName = "UnlockTile";
        GameObject prefab = PrefabBuildingList.Find(go => go.name == buildingPrefabName);
        int centerX = width / 2;
        int centerZ = height / 2;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if ((x == centerX || x == centerX - 1) && (z == centerZ || z == centerZ - 1))
                {
                    continue;
                }
                buildingData[new Vector2(x, z)] = new UnlockTile();
                buildingGrafik[new Vector3(x, 1, z)] = unlockTilePrefab;
            }
        }
        buildingData.OnDictionaryChanged += GameplayManager.calculateOutput;
        buildingGrafik.OnDictionaryChanged += GameplayManager.renderBuildings;
    }
    private void GenerateBuildingData()
    {
        List<ResearchNode> PossibleResearch = new List<ResearchNode>();
        //Hut Creation
        string buildingPrefabName = "Hut";
        GameObject prefab = PrefabBuildingList.Find(go => go.name == buildingPrefabName);
        BuildingExtraData Hut = new BuildingExtraData(InfoPrefab, ResearchPrefab, BuildBuidlingPrefab, new Hut(), prefab, buildingPrefabName, $"{buildingPrefabName}:");
        ResearchNode HutNode = new ResearchNode($"{buildingPrefabName}:", buildingPrefabName);
        Hut.BuildingNode = HutNode;
        PossibleResearch.Add(HutNode);
        BuildingDataList.Add(Hut);
        //Farm Creation
        buildingPrefabName = "Farm";
        prefab = PrefabBuildingList.Find(go => go.name == buildingPrefabName);
        BuildingExtraData Farm = new BuildingExtraData(InfoPrefab, ResearchPrefab, BuildBuidlingPrefab, new Farm(), prefab, buildingPrefabName, $"{buildingPrefabName}:");
        ResearchNode FarmNode = new ResearchNode($"{buildingPrefabName}:", buildingPrefabName);
        Farm.BuildingNode = FarmNode;
        PossibleResearch.Add(FarmNode);
        BuildingDataList.Add(Farm);
        //Mine Creation
        buildingPrefabName = "Mine";
        prefab = PrefabBuildingList.Find(go => go.name == buildingPrefabName);
        BuildingExtraData Mine = new BuildingExtraData(InfoPrefab, ResearchPrefab, BuildBuidlingPrefab, new Mine(), prefab, buildingPrefabName, $"{buildingPrefabName}:");
        ResearchNode MineNode = new ResearchNode($"{buildingPrefabName}:", buildingPrefabName);
        Mine.BuildingNode = MineNode;
        PossibleResearch.Add(MineNode);
        BuildingDataList.Add(Mine);
        //Workshop Creation
        buildingPrefabName = "Workshop";
        prefab = PrefabBuildingList.Find(go => go.name == buildingPrefabName);
        BuildingExtraData Workshop = new BuildingExtraData(InfoPrefab, ResearchPrefab, BuildBuidlingPrefab, new Workshop(), prefab, buildingPrefabName, $"{buildingPrefabName}:");
        ResearchNode WorkshopNode = new ResearchNode($"{buildingPrefabName}:", buildingPrefabName);
        Workshop.BuildingNode = WorkshopNode;
        PossibleResearch.Add(WorkshopNode);
        BuildingDataList.Add(Workshop);
        //School Creation
        buildingPrefabName = "School";
        prefab = PrefabBuildingList.Find(go => go.name == buildingPrefabName);
        BuildingExtraData School = new BuildingExtraData(InfoPrefab, ResearchPrefab, BuildBuidlingPrefab, new School(), prefab, buildingPrefabName, $"{buildingPrefabName}:");
        ResearchNode SchoolNode = new ResearchNode($"{buildingPrefabName}:", buildingPrefabName);
        School.BuildingNode = SchoolNode;
        PossibleResearch.Add(SchoolNode);
        BuildingDataList.Add(School);
        //Park Creation
        buildingPrefabName = "Park";
        prefab = PrefabBuildingList.Find(go => go.name == buildingPrefabName);
        BuildingExtraData Park = new BuildingExtraData(InfoPrefab, ResearchPrefab, BuildBuidlingPrefab, new Park(), prefab, buildingPrefabName, $"{buildingPrefabName}:");
        ResearchNode ParkNode = new ResearchNode($"{buildingPrefabName}:", buildingPrefabName, 600, new List<ResearchNode>());
        Park.BuildingNode = ParkNode;
        PossibleResearch.Add(ParkNode);
        BuildingDataList.Add(Park);
        //House Creation
        buildingPrefabName = "House";
        prefab = PrefabBuildingList.Find(go => go.name == buildingPrefabName);
        BuildingExtraData House = new BuildingExtraData(InfoPrefab, ResearchPrefab, BuildBuidlingPrefab, new House(), prefab, buildingPrefabName, $"{buildingPrefabName}:");
        ResearchNode HouseNode = new ResearchNode($"{buildingPrefabName}:", buildingPrefabName, 600, new List<ResearchNode>());
        House.BuildingNode = HouseNode;
        PossibleResearch.Add(HouseNode);
        BuildingDataList.Add(House); ;
        //CoalPowerPlant Creation
        buildingPrefabName = "CoalPowerPlant";
        prefab = PrefabBuildingList.Find(go => go.name == buildingPrefabName);
        BuildingExtraData CoalPowerPlant = new BuildingExtraData(InfoPrefab, ResearchPrefab, BuildBuidlingPrefab, new CoalPowerPlant(), prefab, buildingPrefabName, $"{buildingPrefabName}:");
        ResearchNode CoalPowerPlantNode = new ResearchNode($"{buildingPrefabName}:", buildingPrefabName, 600, new List<ResearchNode>());
        CoalPowerPlant.BuildingNode = CoalPowerPlantNode;
        PossibleResearch.Add(CoalPowerPlantNode);
        BuildingDataList.Add(CoalPowerPlant);
        //WateredFarm Creation
        buildingPrefabName = "WateredFarm";
        prefab = PrefabBuildingList.Find(go => go.name == buildingPrefabName);
        BuildingExtraData WateredFarm = new BuildingExtraData(InfoPrefab, ResearchPrefab, BuildBuidlingPrefab, new WateredFarm(), prefab, buildingPrefabName, $"{buildingPrefabName}:");
        ResearchNode WateredFarmNode = new ResearchNode($"{buildingPrefabName}:", buildingPrefabName, 800, new List<ResearchNode>());
        WateredFarm.BuildingNode = WateredFarmNode;
        PossibleResearch.Add(WateredFarmNode);
        BuildingDataList.Add(WateredFarm);
        //HighSchool Creation
        buildingPrefabName = "HighSchool";
        prefab = PrefabBuildingList.Find(go => go.name == buildingPrefabName);
        BuildingExtraData HighSchool = new BuildingExtraData(InfoPrefab, ResearchPrefab, BuildBuidlingPrefab, new HighSchool(), prefab, buildingPrefabName, $"{buildingPrefabName}:");
        ResearchNode HighSchoolNode = new ResearchNode($"{buildingPrefabName}:", buildingPrefabName, 800, new List<ResearchNode>());
        HighSchool.BuildingNode = HighSchoolNode;
        PossibleResearch.Add(HighSchoolNode);
        BuildingDataList.Add(HighSchool);
        //OpenPitMining Creation
        buildingPrefabName = "OpenPitMining";
        prefab = PrefabBuildingList.Find(go => go.name == buildingPrefabName);
        BuildingExtraData OpenPitMining = new BuildingExtraData(InfoPrefab, ResearchPrefab, BuildBuidlingPrefab, new OpenPitMining(), prefab, buildingPrefabName, $"{buildingPrefabName}:");
        ResearchNode OpenPitMiningNode = new ResearchNode($"{buildingPrefabName}:", buildingPrefabName, 800, new List<ResearchNode>());
        OpenPitMining.BuildingNode = OpenPitMiningNode;
        PossibleResearch.Add(OpenPitMiningNode);
        BuildingDataList.Add(OpenPitMining);
        //Manufacture Creation
        buildingPrefabName = "Manufacture";
        prefab = PrefabBuildingList.Find(go => go.name == buildingPrefabName);
        BuildingExtraData Manufacture = new BuildingExtraData(InfoPrefab, ResearchPrefab, BuildBuidlingPrefab, new Manufacture(), prefab, buildingPrefabName, $"{buildingPrefabName}:");
        ResearchNode ManufactureNode = new ResearchNode($"{buildingPrefabName}:", buildingPrefabName, 800, new List<ResearchNode>());
        Manufacture.BuildingNode = ManufactureNode;
        PossibleResearch.Add(ManufactureNode);
        BuildingDataList.Add(Manufacture);
        //Possible Research
        GameplayManager.PossibleResearch = PossibleResearch;
    }
    private void CreateInfoPanels()
    {
        List<GameObject> buildingPanelList = new List<GameObject>();
        Transform BuildingSelectTransform = SidePanel.transform.Find("BuildingSelect");
        buildingPanelList.Add(BuildingSelectTransform.gameObject);
        BuildingSelectTransform = SidePanel.transform.Find("UnlockTile(Info)");
        buildingPanelList.Add(BuildingSelectTransform.gameObject);
        foreach (BuildingExtraData buildingdefinition in BuildingDataList)
        {
            GameObject buildingpanel = buildingdefinition.InfoPrefab;
            GameObject newPanel = Instantiate(buildingpanel, SidePanel.transform);
            newPanel.name = $"{buildingdefinition.Name}(Info)";
            newPanel.GetComponent<InfoPrefab>().InfoPrefabVariables(GameplayManager, buildingdefinition.TheBuidling, buildingdefinition.Infotext);
            buildingPanelList.Add(newPanel);
        }
        GameplayManager.buildingPanelList = buildingPanelList;
    }
    private void CreateBuildingPanels()
    {
        List<GameObject> buildBuildingPanelList = new List<GameObject>();
        foreach (BuildingExtraData buildingdefinition in BuildingDataList)
        {
            GameObject buildingpanel = buildingdefinition.BuildBuidlingPrefab;
            GameObject newPanel = Instantiate(buildingpanel, BuildPanel.transform);
            newPanel.name = $"Build{buildingdefinition.Name}Panel";
            newPanel.GetComponent<BuildBuidlingPrefab>().BuildBuidlingPrefabVariables(GameplayManager, buildingdefinition.BuildingPrefab, maincam.gameObject, buildingdefinition.TheBuidling);
            buildBuildingPanelList.Add(newPanel);
            buildingdefinition.BuildingNode.ResearchItem = newPanel;
        }
        GameplayManager.buildBuildingPanelList = buildBuildingPanelList;
    }
    private void CreateResearchPanels()
    {
        foreach (BuildingExtraData buildingdefinition in BuildingDataList)
        {
            GameObject buildingpanel = buildingdefinition.ResearchPrefab;
            GameObject newPanel = Instantiate(buildingpanel, ResearchPanel.transform);
            newPanel.name = $"Research{buildingdefinition.Name}";
            newPanel.GetComponent<ResearchPrefab>().ResearchPrefabVariables(GameplayManager, buildingdefinition.TheBuidling, buildingdefinition.BuildingNode);
            buildingdefinition.BuildingNode.ResearchDisplay = newPanel;
        }
    }
}
