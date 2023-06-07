using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoUnlockTile : MonoBehaviour
{
    public GameplayManager GameplayManager;
    private TextMeshProUGUI myTextMeshPro;
    private Button buildButton;
    void Start()
    {
        myTextMeshPro = transform.Find("BuildingDescription").GetComponent<TextMeshProUGUI>();
        buildButton = transform.Find("RemoveButton").GetComponent<Button>();
        buildButton.onClick.AddListener(TaskOnClick);
        textUpdate();
        GameplayManager.OnCloseAllPanels += textUpdate;
    }
    public virtual void textUpdate()
    {
        building value = new UnlockTile();
        int UnlockTiles = GameplayManager.buildingData.Values.Count(v => v is UnlockTile);
        int production = GameplayManager.MaterialGrowth(value.production, DecreasingFunction(UnlockTiles,GameplayManager.width,GameplayManager.height));
        string description = $"Unlock Tile: for {production}";
        myTextMeshPro.text = description;
    }
    private void TaskOnClick()
    {
        building value;
        GameplayManager.buildingData.TryGetValue(new Vector2(GameplayManager.highXcur, GameplayManager.highZcur), out value);
        if (value is UnlockTile)
        {
            Debug.Log($"True:{value}");
            int UnlockTiles = GameplayManager.buildingData.Values.Count(v => v is UnlockTile);
            int production = GameplayManager.MaterialGrowth(value.production, DecreasingFunction(UnlockTiles, GameplayManager.width, GameplayManager.height));
            GameplayManager.CloseAllPanels();
            GameplayManager.UnlockTileLoad(production,GameplayManager.highXcur,GameplayManager.highZcur);
        }
        else
        {
            Debug.Log($"False:{value}");
        }
    }
    int DecreasingFunction(int input, int width, int height)
    {
        int constant = (width+1) * (height+1)-4;
        int nextconstant = GameplayManager.MaterialGrowth(constant - input,5);
        return nextconstant;
    }
}
