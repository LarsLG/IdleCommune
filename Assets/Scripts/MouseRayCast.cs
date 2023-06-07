using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class MouseRayCast : MonoBehaviour
{
    Camera cam;
    [SerializeField] private GameplayManager GameplayManager;
    [SerializeField] private float hight;
    [SerializeField] private GameObject HighlightPrefab;
    
    public GameObject buildPanel;
    private float moveSpeed = 200;
    private float minCamera = 5;
    private float maxCamera = 30;
    private char specialChar = '(';

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 100f;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.blue);

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on the UI");
            }
            else
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    try
                    {
                        Debug.Log(hit.collider);
                        GameplayManager.CloseAllPanels();
                        //Set Highliter for selection
                        DestroyAllHighlights();
                        ToggleHighlight((int)hit.collider.transform.position.x, (int)hit.collider.transform.position.z);

                        if (hit.collider.tag == "GameMapObject")
                        {
                            buildPanel.SetActive(true);
                        }
                        else
                        {
                            buildPanel.SetActive(false);
                            //Building detailed Description
                            foreach (GameObject gameObject in GameplayManager.buildingPanelList)
                            {
                                string[] gameObjectNameParts = gameObject.name.Split(specialChar);
                                string gameObjectName = gameObjectNameParts[0];
                                if (gameObjectName == hit.collider.name.Split(specialChar)[0])
                                {
                                    if (gameObjectName == "UnlockTile")
                                    {
                                        List<Vector2> Ckecklist = new List<Vector2> { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };

                                        foreach (Vector2 pos in Ckecklist)
                                        {
                                            Vector2 posEffect = new Vector2(GameplayManager.highXcur, GameplayManager.highZcur) + pos;
                                            if (GameplayManager.buildingData.TryGetValue(posEffect, out building effector))
                                            {
                                                if (effector is not UnlockTile)
                                                {
                                                    gameObject.SetActive(true);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        gameObject.SetActive(true);
                                    }
                                }
                            }
                        }
                    }
                    catch (System.Exception)
                    {
                        throw new Exception("Somthing went Wrong");
                    }

                }
            }   
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("Scroll on the UI");
        }
        else
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && transform.position.y > minCamera) // forward
            {
                transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f && transform.position.y < maxCamera) // backwards
            {
                transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
            }
        }
    }

    public void ToggleHighlight(int curX, int curZ)
    {
            Instantiate(HighlightPrefab, new Vector3(curX, hight, curZ), HighlightPrefab.transform.rotation);
            //Debug.Log($"{curX},{curZ} Raycast");
            GameplayManager.highXcur = curX;
            GameplayManager.highZcur = curZ;
    }
    public void HighlightEffct(int curX, int curZ, float color)
    {
        var highlight = Instantiate(HighlightPrefab, new Vector3(curX, hight, curZ), HighlightPrefab.transform.rotation);
        highlight.GetComponent<HighlightScript>().Init(color);
    }

    public void DestroyAllHighlights()
    {
        string objectName = "Highlight(Clone)"; // replace with the name of the object you want to delete
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name == objectName)
            {
                Destroy(gameObject);
            }
        }
    }
}

