using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{

    public static SaveData data = new SaveData();

    public static void SaveLevel(Chart chart)
    {
        string path = Application.persistentDataPath + chart.song.name + ".level";

        string json = JsonUtility.ToJson(chart, true);

        File.WriteAllText(path, json);
    }

    public static Chart LoadChart(string path)
    {
        Chart chart = ScriptableObject.CreateInstance<Chart>();
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, chart);
        }
        
        return chart;

    }

    public static void SaveData(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SaveFile", json);
        Debug.Log(PlayerPrefs.GetString("SaveFile"));
    }

    public static SaveData LoadGame()
    {
        data = new SaveData();
        if (PlayerPrefs.HasKey("SaveFile"))
        {
            data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("SaveFile"));
            data.correct = true;
        }
        Debug.Log(data.money);
        return data;
    }

    public static void DeleteSaveData()
    {
        if (PlayerPrefs.HasKey("SaveFile")) PlayerPrefs.DeleteKey("SaveFile");
    }
}




public struct SaveData
{
    public int money;
    public bool[] unlockedCosmetics;
    public List<ChartData> allCharts;
    public bool correct;

    public SaveData(bool correct = false) : this()
    {
        this.correct = correct;
    }
}
