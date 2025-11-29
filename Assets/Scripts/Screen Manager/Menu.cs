using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private string _optionsName = "Options";
    [SerializeField] private string _shopName = "Shop";
    public static bool saved = false;

    private void Awake()
    {
        if (saved) return;

        Debug.Log("loading");
        SaveData data = SaveManager.LoadGame();
        saved = true;
        ChartDataHolder.allCharts = new();
        MoneyManager.money = data.money;
        InventoryManager.unlockedCosmetics = data.unlockedCosmetics;
        if (data.allCharts.Length <= 0) return;
        foreach (var c in data.allCharts) ChartDataHolder.allCharts.Add(JsonUtility.FromJson<ChartData>(c));
    }

    public void Exit() => Application.Quit();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("loading");
            SaveData data = SaveManager.LoadGame();
            saved = true;
            ChartDataHolder.allCharts = new();
            foreach (var c in data.allCharts) ChartDataHolder.allCharts.Add(JsonUtility.FromJson<ChartData>(c));
            MoneyManager.money = data.money;
            InventoryManager.unlockedCosmetics = data.unlockedCosmetics;
        }
    }

    public void DeleteAll()
    {
        SaveManager.DeleteSaveData();
        SaveData data = SaveManager.LoadGame();
        Debug.Log("loading");
        saved = true;
        ChartDataHolder.allCharts = new();
        //foreach (var c in data.allCharts) ChartDataHolder.allCharts.Add(JsonUtility.FromJson<ChartData>(c));
        MoneyManager.money = data.money;
        InventoryManager.unlockedCosmetics = data.unlockedCosmetics;

    }

    public void Options() => ScreenManager.instance.Push(_optionsName);

    public void Shop() => ScreenManager.instance.Push(_shopName);
}
