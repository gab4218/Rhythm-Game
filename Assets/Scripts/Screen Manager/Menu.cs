using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private string _optionsName = "Options";
    [SerializeField] private string _shopName = "Shop";
    public static bool saved = false;
    public static int selectedVisuals = 0;

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

    public void SelectVisuals(TMP_Dropdown v)
    {
        selectedVisuals = v.value;
        ScreenManager.instance.OnClick(v.transform);
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

    public void Options(Transform p)
    {
        ScreenManager.instance.Push(_optionsName);
        ScreenManager.instance.OnClick(p);

    }

    public void Shop(Transform p)
    {
        ScreenManager.instance.Push(_shopName);
        ScreenManager.instance.OnClick(p);
    }
}
