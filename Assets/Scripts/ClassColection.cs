using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ClassColection : MonoBehaviour
{}
public enum Tags
{
    Livingspace,
    Happiness,
    Farming,
    WaterFarm,
    VerticalFarm,
    AnimalFarm,
    Education,
    Research,
    CentralPlanning,
    Production,
    Energy,
    NonFossileFuel,
    FossileFuel,
    Material,
    Mining,
    MaterialGrowing
}
public enum GVM
{
    MaterialBaseProd,
    FoodBaseProd,
    WorkBaseProd,
    ResearchBaseProd,

    MaterialWorkerProd,
    FoodWorkerProd,
    WorkWorkerProd,
    ResearchWorkerProd,

    MaterialUse,
    WorkerMaterialUse,
    EnergyUse,
    FoodUse,
    WorkerFoodUse,

    LivingSpace,
    MaxWorker,
    Happiness,
}
public class building 
{
    //name
    public string name { get; set; }
    //cost
    public int matrials { get; set; }
    public int production { get; set; }

    //stats
    public float MaterialBaseProd { get; set; }
    public float FoodBaseProd { get; set; }
    public float WorkBaseProd { get; set; }
    public float ResearchBaseProd { get; set; }

    public float MaterialWorkerProd { get; set; }
    public float FoodWorkerProd { get; set; }
    public float WorkWorkerProd { get; set; }
    public float ResearchWorkerProd { get; set; }

    public float MaterialUse { get; set; }
    public float WorkerMaterialUse { get; set; }
    public float EnergyUse { get; set; }
    public float FoodUse { get; set; }
    public float WorkerFoodUse { get; set; }

    public int LivingSpace { get; set; }
    public int MaxWorker { get; set; }
    public int CurrentWorker { get; set; }
    public float Happiness { get; set; }
    public List<Vector2> EffectAera;
    public List<Tags> BuildingTags;
    public List<MyTuple<Tags, GVM, float>> RessourceEffect;

    public building(string namec, int matrialsc, int productionc, float MaterialBaseProdc, float FoodBaseProdc, float WorkBaseProdc, float ResearchBaseProdc, float MaterialWorkerProdc,
    float FoodWorkerProdc, float WorkWorkerProdc, float ResearchWorkerProdc, float MaterialUsec, float WorkerMaterialUsec, float EnergyUsec, float FoodUsec, float WorkerFoodUsec, int LivingSpacec, int MaxWorkerc, int CurrentWorkerc, float Happinessc,
    List<Vector2> EffectAerac, List<Tags> BuildingTagsc, List<MyTuple<Tags, GVM, float>> RessourceEffectc)
    {
        name = namec;
        matrials = matrialsc;
        production = productionc;
        MaterialBaseProd = MaterialBaseProdc;
        FoodBaseProd = FoodBaseProdc;
        WorkBaseProd = WorkBaseProdc;
        ResearchBaseProd = ResearchBaseProdc;
        MaterialWorkerProd = MaterialWorkerProdc;
        FoodWorkerProd = FoodWorkerProdc;
        WorkWorkerProd = WorkWorkerProdc;
        ResearchWorkerProd = ResearchWorkerProdc;
        MaterialUse = MaterialUsec;
        WorkerMaterialUse = WorkerMaterialUsec;
        EnergyUse = EnergyUsec;
        FoodUse = FoodUsec;
        WorkerFoodUse = WorkerFoodUsec;
        LivingSpace = LivingSpacec;
        MaxWorker = MaxWorkerc;
        CurrentWorker = CurrentWorkerc;
        Happiness = Happinessc;
        EffectAera = EffectAerac;
        BuildingTags = BuildingTagsc;
        RessourceEffect = RessourceEffectc;
    }
}
public class constructionSite : building
{
    public GameplayManager GameplayManager;
    private int highXcur, highZcur;
    private building toBuilding;
    private GameObject buildingPrefab;
    public constructionSite() : base(
        namec: "constructionSite",
        matrialsc: 0,
        productionc: 0,
        MaterialBaseProdc: 0,
        FoodBaseProdc: 0,
        WorkBaseProdc: 0,
        ResearchBaseProdc: 0,
        MaterialWorkerProdc: 0,
        FoodWorkerProdc: 0,
        WorkWorkerProdc: 0,
        ResearchWorkerProdc: 0,
        MaterialUsec: 0,
        WorkerMaterialUsec: 0,
        EnergyUsec: 0,
        WorkerFoodUsec: 0,
        FoodUsec: 0,
        LivingSpacec: 0,
        MaxWorkerc: 0,
        CurrentWorkerc: 0,
        Happinessc: 0,
        EffectAerac:
        new List<Vector2> { },
        BuildingTagsc:
        new List<Tags> { },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>> { })
    { }
    public constructionSite(GameObject finshedPrefab, building finshed, GameplayManager Manger, int Xcur, int Zcur) : base(
        namec: "constructionSite",
        matrialsc: 0,
        productionc: 0,
        MaterialBaseProdc: 0,
        FoodBaseProdc: 0,
        WorkBaseProdc: 0,
        ResearchBaseProdc: 0,
        MaterialWorkerProdc: 0,
        FoodWorkerProdc: 0,
        WorkWorkerProdc: 0,
        ResearchWorkerProdc: 0,
        MaterialUsec: 0,
        WorkerMaterialUsec: 0,
        EnergyUsec: 0,
        WorkerFoodUsec: 0,
        FoodUsec: 0,
        LivingSpacec: 0,
        MaxWorkerc: 0,
        CurrentWorkerc: 0,
        Happinessc: 0,
        EffectAerac:
        new List<Vector2> { },
        BuildingTagsc:
        new List<Tags> { },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>> { })
    {
        GameplayManager = Manger;
        toBuilding = finshed;
        buildingPrefab = finshedPrefab;
        highXcur = Xcur;
        highZcur = Zcur;
        int production = GameplayManager.MaterialGrowth(toBuilding.production, GameplayManager.BuildingCount);
        GameplayManager.BuildingCocunt(production, toBuilding, buildingPrefab, highXcur, highZcur);
    }
}
public class UnlockTile : building
{
    public GameplayManager GameplayManager;
    private int highXcur, highZcur;
    public UnlockTile() : base(
        namec: "UnlockTile",
        matrialsc: 0,
        productionc: 20,
        MaterialBaseProdc: 0,
        FoodBaseProdc: 0,
        WorkBaseProdc: 0,
        ResearchBaseProdc: 0,
        MaterialWorkerProdc: 0,
        FoodWorkerProdc: 0,
        WorkWorkerProdc: 0,
        ResearchWorkerProdc: 0,
        MaterialUsec: 0,
        WorkerMaterialUsec: 0,
        EnergyUsec: 0,
        WorkerFoodUsec: 0,
        FoodUsec: 0,
        LivingSpacec: 0,
        MaxWorkerc: 0,
        CurrentWorkerc: 0,
        Happinessc: 0,
        EffectAerac:
        new List<Vector2> { },
        BuildingTagsc:
        new List<Tags> { },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>> { })
    { }
}
public class Hut : building
{
    public Hut() : base(
    namec: "Hut",
    matrialsc: 15,
    productionc: 0,
    MaterialBaseProdc: 0,
    FoodBaseProdc: 0,
    WorkBaseProdc: 0,
    ResearchBaseProdc: 0,
    MaterialWorkerProdc: 0,
    FoodWorkerProdc: 0,
    WorkWorkerProdc: 0,
    ResearchWorkerProdc: 0,
    MaterialUsec: 0,
    WorkerMaterialUsec: 0,
    EnergyUsec: 0,
    FoodUsec: 8,
    WorkerFoodUsec: 0,
    LivingSpacec: 4,
    MaxWorkerc: 0,
    CurrentWorkerc: 0,
    Happinessc: 0,
    EffectAerac:
    new List<Vector2> { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, 1) },
    BuildingTagsc:
    new List<Tags> { Tags.Livingspace },
    RessourceEffectc: new List<MyTuple<Tags, GVM, float>>
    {   new MyTuple<Tags, GVM, float>(Tags.Material, GVM.MaterialWorkerProd, 1.1f),
        new MyTuple<Tags, GVM, float>(Tags.Production, GVM.WorkWorkerProd, 1.1f),
        new MyTuple<Tags, GVM, float>(Tags.Farming, GVM.FoodWorkerProd, 1.1f)
       })
    { }
}
public class Mine : building
{
    public Mine() : base(
        namec: "Mine",
        matrialsc: 15,
        productionc: 0,
        MaterialBaseProdc: 0,
        FoodBaseProdc: 0,
        WorkBaseProdc: 0,
        ResearchBaseProdc: 0,
        MaterialWorkerProdc: 20,
        FoodWorkerProdc: 0,
        WorkWorkerProdc: 0,
        ResearchWorkerProdc: 0,
        MaterialUsec: 0,
        WorkerMaterialUsec: 0,
        EnergyUsec: 0,
        FoodUsec: 0,
        WorkerFoodUsec: 5,
        LivingSpacec: 0,
        MaxWorkerc: 4,
        CurrentWorkerc: 0,
        Happinessc: 0,
        EffectAerac:
        new List<Vector2> { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, 1) },
        BuildingTagsc:
        new List<Tags> { Tags.Material, Tags.Mining },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>>
        {   new MyTuple<Tags, GVM, float>(Tags.Livingspace, GVM.Happiness, 0.9f),
            new MyTuple<Tags, GVM, float>(Tags.FossileFuel, GVM.EnergyUse, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Farming, GVM.FoodWorkerProd, 0.95f),
            new MyTuple<Tags, GVM, float>(Tags.Farming, GVM.FoodBaseProd, 1.05f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.WorkWorkerProd, 1.5f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.WorkBaseProd, 1.5f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.WorkerMaterialUse, 2f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.MaterialUse, 2f)
        })
    { }
}
public class Workshop : building
{
    public Workshop() : base(
        namec: "Workshop",
        matrialsc: 30,
        productionc: 0,
        MaterialBaseProdc: 0,
        FoodBaseProdc: 0,
        WorkBaseProdc: 0,
        ResearchBaseProdc: 0,
        MaterialWorkerProdc: 0,
        FoodWorkerProdc: 0,
        WorkWorkerProdc: 20,
        ResearchWorkerProdc: 0,
        MaterialUsec: 0,
        WorkerMaterialUsec: 1,
        EnergyUsec: 0,
        FoodUsec: 0,
        WorkerFoodUsec: 2,
        LivingSpacec: 0,
        MaxWorkerc: 4,
        CurrentWorkerc: 0,
        Happinessc: 0,
        EffectAerac:
        new List<Vector2> { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, 1) },
        BuildingTagsc:
        new List<Tags> { Tags.Production },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>>
        {   new MyTuple<Tags, GVM, float>(Tags.Livingspace, GVM.Happiness, 0.9f),
            new MyTuple<Tags, GVM, float>(Tags.Energy, GVM.EnergyUse, 1.06f),
            new MyTuple<Tags, GVM, float>(Tags.Material, GVM.FoodUse, 1.05f),
            new MyTuple<Tags, GVM, float>(Tags.Material, GVM.WorkerFoodUse, 1.05f)
        })
    { }
}
public class Farm : building
{
    public Farm() : base(
        namec: "Farm",
        matrialsc: 10,
        productionc: 0,
        MaterialBaseProdc: 0,
        FoodBaseProdc: 0,
        WorkBaseProdc: 0,
        ResearchBaseProdc: 0,
        MaterialWorkerProdc: 0,
        FoodWorkerProdc: 20,
        WorkWorkerProdc: 0,
        ResearchWorkerProdc: 0,
        MaterialUsec: 1,
        WorkerMaterialUsec: 0,
        EnergyUsec: 0,
        WorkerFoodUsec: 5,
        FoodUsec: 0,
        LivingSpacec: 0,
        MaxWorkerc: 4,
        CurrentWorkerc: 0,
        Happinessc: 0,
        EffectAerac:
        new List<Vector2> { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, 1) },
        BuildingTagsc:
        new List<Tags> { Tags.Farming },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>>
        {   new MyTuple<Tags, GVM, float>(Tags.Livingspace, GVM.Happiness, 0.9f),
            new MyTuple<Tags, GVM, float>(Tags.NonFossileFuel, GVM.EnergyUse, 1.1f)
        })
    { }
}
public class School : building
{
    public School() : base(
        namec: "School",
        matrialsc: 80,
        productionc: 60,
        MaterialBaseProdc: 0,
        FoodBaseProdc: 0,
        WorkBaseProdc: 0,
        ResearchBaseProdc: 5,
        MaterialWorkerProdc: 0,
        FoodWorkerProdc: 0,
        WorkWorkerProdc: 0,
        ResearchWorkerProdc: 10,
        MaterialUsec: 0,
        WorkerMaterialUsec: 0,
        EnergyUsec: 0,
        WorkerFoodUsec: 2,
        FoodUsec: 0,
        LivingSpacec: 0,
        MaxWorkerc: 4,
        CurrentWorkerc: 0,
        Happinessc: 0,
        EffectAerac:
        new List<Vector2> { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, 1) },
        BuildingTagsc:
        new List<Tags> { Tags.Education },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>>
        {   new MyTuple<Tags, GVM, float>(Tags.Livingspace, GVM.Happiness, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.WorkWorkerProd, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.WorkerMaterialUse, 0.9f)
        })
    { }
}
public class House : building
{
    public House() : base(
        namec: "House",
        matrialsc: 40,
        productionc: 40,
        MaterialBaseProdc: 0,
        FoodBaseProdc: 0,
        WorkBaseProdc: 0,
        ResearchBaseProdc: 0,
        MaterialWorkerProdc: 0,
        FoodWorkerProdc: 0,
        WorkWorkerProdc: 0,
        ResearchWorkerProdc: 0,
        MaterialUsec: 0,
        WorkerMaterialUsec: 0,
        EnergyUsec: 0,
        FoodUsec: 8,
        WorkerFoodUsec: 0,
        LivingSpacec: 10,
        MaxWorkerc: 0,
        CurrentWorkerc: 0,
        Happinessc: 0,
        EffectAerac:
        new List<Vector2> { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, 1) },
        BuildingTagsc:
        new List<Tags> { Tags.Livingspace },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>>
        {   new MyTuple<Tags, GVM, float>(Tags.Material, GVM.MaterialWorkerProd, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.WorkWorkerProd, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Farming, GVM.FoodWorkerProd, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Education, GVM.ResearchWorkerProd, 1.1f)
           })
    { }
}
public class Park : building
{
    public Park() : base(
        namec: "Park",
        matrialsc: 30,
        productionc: 90,
        MaterialBaseProdc: 0,
        FoodBaseProdc: 0,
        WorkBaseProdc: 0,
        ResearchBaseProdc: 0,
        MaterialWorkerProdc: 0,
        FoodWorkerProdc: 0,
        WorkWorkerProdc: 0,
        ResearchWorkerProdc: 0,
        MaterialUsec: 0,
        WorkerMaterialUsec: 0,
        EnergyUsec: 0,
        FoodUsec: 0,
        WorkerFoodUsec: 0,
        LivingSpacec: 0,
        MaxWorkerc: 0,
        CurrentWorkerc: 0,
        Happinessc: 4,
        EffectAerac:
        new List<Vector2> { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, 1) },
        BuildingTagsc:
        new List<Tags> { Tags.Happiness },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>>
        {   new MyTuple<Tags, GVM, float>(Tags.Livingspace, GVM.Happiness, 1.4f),
            new MyTuple<Tags, GVM, float>(Tags.Education, GVM.ResearchWorkerProd, 1.1f)
           })
    { }
}
public class CoalPowerPlant : building
{
    public CoalPowerPlant() : base(
        namec: "CoalPowerPlant",
        matrialsc: 80,
        productionc: 120,
        MaterialBaseProdc: 0,
        FoodBaseProdc: 0,
        WorkBaseProdc: 0,
        ResearchBaseProdc: 0,
        MaterialWorkerProdc: 0,
        FoodWorkerProdc: 0,
        WorkWorkerProdc: 0,
        ResearchWorkerProdc: 0,
        MaterialUsec: 4,
        WorkerMaterialUsec: 0,
        EnergyUsec: 20,
        FoodUsec: 0,
        WorkerFoodUsec: 0,
        LivingSpacec: -1,
        MaxWorkerc: 0,
        CurrentWorkerc: 0,
        Happinessc: 0,
        EffectAerac:
        new List<Vector2> { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(2, 0), new Vector2(0, 2), new Vector2(-2, 0), new Vector2(0, -2) },
        BuildingTagsc:
        new List<Tags> { Tags.Energy, Tags.FossileFuel },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>>
        {   new MyTuple<Tags, GVM, float>(Tags.Livingspace, GVM.Happiness, 0.8f),
            new MyTuple<Tags, GVM, float>(Tags.Happiness, GVM.Happiness, 0.8f),
            new MyTuple<Tags, GVM, float>(Tags.FossileFuel, GVM.EnergyUse, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Material, GVM.MaterialBaseProd, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Farming, GVM.FoodBaseProd, 0.8f),
            new MyTuple<Tags, GVM, float>(Tags.Farming, GVM.FoodWorkerProd, 0.8f)
           })
    { }
}
public class WateredFarm : building
{
    public WateredFarm() : base(
        namec: "WateredFarm",
        matrialsc: 70,
        productionc: 200,
        MaterialBaseProdc: 0,
        FoodBaseProdc: 0,
        WorkBaseProdc: 0,
        ResearchBaseProdc: 0,
        MaterialWorkerProdc: 0,
        FoodWorkerProdc: 40,
        WorkWorkerProdc: 0,
        ResearchWorkerProdc: 0,
        MaterialUsec: 4,
        WorkerMaterialUsec: 2,
        EnergyUsec: 0,
        FoodUsec: 0,
        WorkerFoodUsec: 0,
        LivingSpacec: 0,
        MaxWorkerc: 4,
        CurrentWorkerc: 0,
        Happinessc: 0,
        EffectAerac:
        new List<Vector2> { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, 1)},
        BuildingTagsc:
        new List<Tags> { Tags.Farming, Tags.WaterFarm },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>>
        {   new MyTuple<Tags, GVM, float>(Tags.Livingspace, GVM.Happiness, 0.95f),
            new MyTuple<Tags, GVM, float>(Tags.Happiness, GVM.Happiness, 0.95f),
            new MyTuple<Tags, GVM, float>(Tags.Farming, GVM.FoodBaseProd, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Farming, GVM.FoodWorkerProd, 1.1f)
           })
    { }
}
public class HighSchool : building
{
    public HighSchool() : base(
        namec: "HighSchool",
        matrialsc: 140,
        productionc: 280,
        MaterialBaseProdc: 0,
        FoodBaseProdc: 0,
        WorkBaseProdc: 0,
        ResearchBaseProdc: 10,
        MaterialWorkerProdc: 0,
        FoodWorkerProdc: 0,
        WorkWorkerProdc: 0,
        ResearchWorkerProdc: 15,
        MaterialUsec: 1,
        WorkerMaterialUsec: 0,
        EnergyUsec: -3,
        FoodUsec: 0,
        WorkerFoodUsec: 0,
        LivingSpacec: -1,
        MaxWorkerc: 4,
        CurrentWorkerc: 0,
        Happinessc: 0,
        EffectAerac:
        new List<Vector2> { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(2, 0), new Vector2(0, 2), new Vector2(-2, 0), new Vector2(0, -2), new Vector2(2, 1), new Vector2(1, 2), new Vector2(-2, 1), new Vector2(1, -2), new Vector2(2, -1), new Vector2(-1, 2), new Vector2(-2, -1), new Vector2(-1, -2), new Vector2(2, 2), new Vector2(-2, 2), new Vector2(-2, 2), new Vector2(-2, -2) },
        BuildingTagsc:
        new List<Tags> { Tags.Education, Tags.Research },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>>
        {   new MyTuple<Tags, GVM, float>(Tags.Livingspace, GVM.Happiness, 1.2f),
            new MyTuple<Tags, GVM, float>(Tags.Happiness, GVM.Happiness, 1.2f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.EnergyUse, 0.9f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.WorkBaseProd, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.WorkWorkerProd, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Farming, GVM.FoodBaseProd, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Farming, GVM.FoodWorkerProd, 1.1f)
           })
    { }
}
public class OpenPitMining : building
{
    public OpenPitMining() : base(
        namec: "OpenPitMining",
        matrialsc: 40,
        productionc: 300,
        MaterialBaseProdc: 10,
        FoodBaseProdc: 0,
        WorkBaseProdc: 0,
        ResearchBaseProdc: 0,
        MaterialWorkerProdc: 50,
        FoodWorkerProdc: 0,
        WorkWorkerProdc: 0,
        ResearchWorkerProdc: 0,
        MaterialUsec: 0,
        WorkerMaterialUsec: 0,
        EnergyUsec: -6,
        FoodUsec: 0,
        WorkerFoodUsec: 0,
        LivingSpacec: 0,
        MaxWorkerc: 8,
        CurrentWorkerc: 0,
        Happinessc: 0,
        EffectAerac:
        new List<Vector2> { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(2, 0), new Vector2(0, 2), new Vector2(-2, 0), new Vector2(0, -2) },
        BuildingTagsc:
        new List<Tags> { Tags.Material, Tags.Mining },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>>
        {   new MyTuple<Tags, GVM, float>(Tags.Livingspace, GVM.Happiness, 0.8f),
            new MyTuple<Tags, GVM, float>(Tags.Happiness, GVM.Happiness, 0.8f),
            new MyTuple<Tags, GVM, float>(Tags.FossileFuel, GVM.EnergyUse, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.WorkWorkerProd, 1.5f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.WorkBaseProd, 1.5f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.WorkerMaterialUse, 2f),
            new MyTuple<Tags, GVM, float>(Tags.Production, GVM.MaterialUse, 2f),
            new MyTuple<Tags, GVM, float>(Tags.Farming, GVM.FoodBaseProd, 0.8f),
            new MyTuple<Tags, GVM, float>(Tags.Farming, GVM.FoodWorkerProd, 0.8f)
           })
    { }
}
public class Manufacture : building
{
    public Manufacture() : base(
        namec: "Manufacture",
        matrialsc: 200,
        productionc: 300,
        MaterialBaseProdc: 0,
        FoodBaseProdc: 0,
        WorkBaseProdc: 10,
        ResearchBaseProdc: 0,
        MaterialWorkerProdc: 0,
        FoodWorkerProdc: 0,
        WorkWorkerProdc: 45,
        ResearchWorkerProdc: 0,
        MaterialUsec: 5,
        WorkerMaterialUsec: 8,
        EnergyUsec: -6,
        FoodUsec: 0,
        WorkerFoodUsec: 0,
        LivingSpacec: 0,
        MaxWorkerc: 8,
        CurrentWorkerc: 0,
        Happinessc: 0,
        EffectAerac:
        new List<Vector2> { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(2, 0), new Vector2(0, 2), new Vector2(-2, 0), new Vector2(0, -2), new Vector2(2, 1), new Vector2(1, 2), new Vector2(-2, 1), new Vector2(1, -2), new Vector2(2, -1), new Vector2(-1, 2), new Vector2(-2, -1), new Vector2(-1, -2), new Vector2(2, 2), new Vector2(-2, 2), new Vector2(-2, 2), new Vector2(-2, -2) },
        BuildingTagsc:
        new List<Tags> { Tags.Production },
        RessourceEffectc: new List<MyTuple<Tags, GVM, float>>
        {   new MyTuple<Tags, GVM, float>(Tags.Material, GVM.MaterialBaseProd, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Material, GVM.MaterialWorkerProd, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Mining, GVM.EnergyUse, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Mining, GVM.MaterialBaseProd, 1.1f),
            new MyTuple<Tags, GVM, float>(Tags.Mining, GVM.MaterialWorkerProd, 1.1f)
           })
    { }
}
public class BuildingExtraData
{
    public GameObject InfoPrefab;
    public GameObject ResearchPrefab;
    public GameObject BuildBuidlingPrefab;
    public building TheBuidling;
    public GameObject BuildingPrefab;
    public ResearchNode BuildingNode;
    public string Name;
    public string Infotext;
    public BuildingExtraData(
        GameObject infoPrefab,
        GameObject researchPrefab,
        GameObject buildBuildingPrefab,
        building theBuilding,
        GameObject buildingPrefab,
        string name,
        string infoText)
    {
        InfoPrefab = infoPrefab;
        ResearchPrefab = researchPrefab;
        BuildBuidlingPrefab = buildBuildingPrefab;
        TheBuidling = theBuilding;
        BuildingPrefab = buildingPrefab;
        Name = name;
        Infotext = infoText;
    }
}
public class ResearchNode
{
    public string ResearchName;
    public string ResearchID;
    public bool IsCompleted = false;
    public bool IsActive = false;
    public int ResearchNeeded;
    public GameObject ResearchItem;
    public GameObject ResearchDisplay;
    public List<ResearchNode> IngoingEdges;
    public ResearchNode(string name, string id)
    {
        ResearchName = name;
        ResearchID = id;
    }
    public ResearchNode(string name, string id, int needed, List<ResearchNode> edge)
    {
        ResearchName = name;
        ResearchID = id;
        ResearchNeeded = needed;
        IngoingEdges = edge;
    }
    public void ActivityState()
    {
        if(IngoingEdges != null)
        {
            bool allCompletedTrue = IngoingEdges.All(item => item.IsCompleted);
            if (allCompletedTrue)
            {
                IsActive = true;
            }
            else
            {
                IsActive = false;
            }
        }
    }
    public void CompletionState()
    {
        if (IngoingEdges == null)
        {
            IsCompleted = true;
        }
    }
}
public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    public delegate void OnDictionaryChangedDelegate();

    public event OnDictionaryChangedDelegate OnDictionaryChanged;

    public new void Add(TKey key, TValue value)
    {
        base.Add(key, value);
        NotifyDictionaryChanged();
    }

    public new bool Remove(TKey key)
    {
        bool result = base.Remove(key);
        if (result)
        {
            NotifyDictionaryChanged();
        }
        return result;
    }

    public new TValue this[TKey key]
    {
        get
        {
            return base[key];
        }
        set
        {
            base[key] = value;
            NotifyDictionaryChanged();
            //Debug.Log($"ObservableDictionary Replacment{key},{value}");
        }
    }

    private void NotifyDictionaryChanged()
    {
        if (OnDictionaryChanged != null)
        {
            OnDictionaryChanged();
        }
    }
}

public class MyTuple<T1, T2, T3>
{
    public T1 I1 { get; set; }
    public T2 I2 { get; set; }
    public T3 I3 { get; set; }

    public MyTuple(T1 item1, T2 item2, T3 item3)
    {
        I1 = item1;
        I2 = item2;
        I3 = item3;
    }
}
public class BuildBuilding : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected GameplayManager GameplayManager;
    [SerializeField] protected GameObject buildingPrefab;
    [SerializeField] protected GameObject MainCamera;
    protected TextMeshProUGUI myTextMeshPro;
    protected Button buildButton;
    protected building building;

    protected void Start()
    {
        myTextMeshPro = transform.Find("BuildingDescription").GetComponent<TextMeshProUGUI>();
        buildButton = transform.Find("Button").GetComponent<Button>();
        buildButton.onClick.AddListener(TaskOnClick);
        textUpdate();
        GameplayManager.OnCloseAllPanels += textUpdate;
    }
    protected virtual void textUpdate()
    {
        int matrials = GameplayManager.MaterialGrowth(building.matrials, GameplayManager.BuildingCount);
        int production = GameplayManager.MaterialGrowth(building.production, GameplayManager.BuildingCount);
        //Debug.Log(matrials);
        string description = $"Build {building.name}:\n" +
        $"Material Cost: {matrials} \t" +
        $"Work Needed: {production} \n" +
        $"Living Space:  \t";
        myTextMeshPro.text = description;
    }
    protected void TaskOnClick()
    {
        building value;
        int matrials = GameplayManager.MaterialGrowth(building.matrials, GameplayManager.BuildingCount);
        int production = GameplayManager.MaterialGrowth(building.production, GameplayManager.BuildingCount);
        GameplayManager.buildingData.TryGetValue(new Vector2(GameplayManager.highXcur, GameplayManager.highZcur), out value);
        if (value == null && GameplayManager.Material >= matrials)
        {
            GameplayManager.Material -= matrials;
            GameplayManager.CloseAllPanels();
            var SourceHighlightScript = MainCamera.GetComponent<MouseRayCast>();
            SourceHighlightScript.DestroyAllHighlights();
            SourceHighlightScript.ToggleHighlight(GameplayManager.highXcur, GameplayManager.highZcur);
            Debug.Log($"True:{value}");
            if (production == 0)
            {
                GameplayManager.buildingData[new Vector2(GameplayManager.highXcur, GameplayManager.highZcur)] = building;
                GameplayManager.buildingGrafik[new Vector3(GameplayManager.highXcur, 1, GameplayManager.highZcur)] = buildingPrefab;
            }
            else
            {
                GameplayManager.buildingData[new Vector2(GameplayManager.highXcur, GameplayManager.highZcur)] = new constructionSite(buildingPrefab,building,GameplayManager,GameplayManager.highXcur,GameplayManager.highZcur);
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
        foreach (Vector2 pos in building.EffectAera)
        {
            Vector2 posEffect = new Vector2(GameplayManager.highXcur,GameplayManager.highZcur) + pos;
            //Check exsistens
            float posint = 0;
            float negint = 0;
            int countint = 0;
            if (GameplayManager.buildingData.TryGetValue(posEffect, out building effector))
            {
                if (effector != null)
                {
                    //check Tags
                    foreach (MyTuple<Tags, GVM, float> item in building.RessourceEffect)
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