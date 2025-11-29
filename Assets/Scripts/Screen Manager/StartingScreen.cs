using UnityEngine;

public class StartingScreen : MonoBehaviour
{
    [SerializeField] private ScreenGO _startingScreen;

    private void Start()
    {
        ScreenManager.instance.Push(_startingScreen);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseScreen.paused)
            {
                ScreenManager.instance.Clear();
            }
        }
    }

    private void OnApplicationQuit() => Save();

    private void OnApplicationPause(bool pause)
    {
        if(pause) Save();
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus) Save();
    }

    private void Save()
    {
        SaveData data = new();
        data.money = MoneyManager.money;
        data.unlockedCosmetics = InventoryManager.unlockedCosmetics;
        //ChartDataHolder.instance.Save();
        string[] ac = new string[ChartDataHolder.allCharts.Count];
        for (int i = 0; i < ac.Length; i++)
        {
            ac[i] = JsonUtility.ToJson(ChartDataHolder.allCharts[i]);
        }
        data.allCharts = ac;
        Debug.Log(ac.Length);
        SaveManager.SaveData(data);
    }
}
