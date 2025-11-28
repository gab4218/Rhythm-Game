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
        ChartDataHolder.allCharts = data.allCharts;
        MoneyManager.money = data.money;
        InventoryManager.unlockedCosmetics = data.unlockedCosmetics;
        ChartDataHolder.instance.Load();
    }

    public void Exit() => Application.Quit();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SaveData data = SaveManager.LoadGame();
            saved = true;
            ChartDataHolder.allCharts = data.allCharts;
            MoneyManager.money = data.money;
            InventoryManager.unlockedCosmetics = data.unlockedCosmetics;
            ChartDataHolder.instance.Load();
        }
    }

    public void DeleteAll()
    {
        SaveManager.DeleteSaveData();
        SaveData data = SaveManager.LoadGame();
        ChartDataHolder.instance.Load();
        ChartDataHolder.instance.Load();
        saved = true;
        ChartDataHolder.allCharts = data.allCharts;
        MoneyManager.money = data.money;
        InventoryManager.unlockedCosmetics = data.unlockedCosmetics;

    }

    public void Options() => ScreenManager.instance.Push(_optionsName);

    public void Shop() => ScreenManager.instance.Push(_shopName);
}
