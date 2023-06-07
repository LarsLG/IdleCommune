using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RessourceCollection : MonoBehaviour
{
    [SerializeField] private GameplayManager GameplayManager;
    //private TextMeshProUGUI FoodCollectionText;
    private Button FoodCollection;
    //private TextMeshProUGUI MaterialCollectionText;
    private Button MaterialCollection;
    public TextMeshProUGUI PopUP;
    private Canvas canvas;
    private string hexColorFood = "#006400";
    private string hexColorMaterial = "#FF8C00";
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        FoodCollection = transform.Find("FoodCollection").GetComponent<Button>();
        MaterialCollection = transform.Find("MaterialCollection").GetComponent<Button>();
        FoodCollection.onClick.AddListener(FoodCollcted);
        MaterialCollection.onClick.AddListener(MaterialCollcted);
    }
    void FoodCollcted()
    {
        GameplayManager.Food += 1;
        TextMeshProUGUI toEat = Instantiate(PopUP, canvas.transform);
        Vector3 pos = FoodCollection.transform.position;
        // Randomize the position by adding a random offset of +/- 30 pixels to each axis
        float offsetX = Random.Range(-30f, 30f);
        float offsetY = Random.Range(-10f, 10f);
        // Apply the new position to the TextMeshPro object
        toEat.transform.position = new Vector3(pos.x + offsetX, pos.y + offsetY, pos.z);
        toEat.raycastTarget = false;
        if (ColorUtility.TryParseHtmlString(hexColorFood, out Color newColor))
        {
            toEat.color = newColor;
        }
    }
    void MaterialCollcted()
    {
        GameplayManager.Material += 1;
        TextMeshProUGUI toEat = Instantiate(PopUP, canvas.transform);
        Vector3 pos = MaterialCollection.transform.position;
        // Randomize the position by adding a random offset of +/- 30 pixels to each axis
        float offsetX = Random.Range(-30f, 30f);
        float offsetY = Random.Range(-10f, 10f);
        // Apply the new position to the TextMeshPro object
        toEat.transform.position = new Vector3(pos.x + offsetX, pos.y + offsetY, pos.z);
        toEat.raycastTarget = false;
        if (ColorUtility.TryParseHtmlString(hexColorMaterial, out Color newColor))
        {
            toEat.color = newColor;
        }
    }
}
