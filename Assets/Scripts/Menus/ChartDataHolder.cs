using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChartDataHolder : MonoBehaviour
{
    public static List<ChartData> allCharts;
    public List<Chart> charts;

    [SerializeField] private ChartSelection _chartPrefab;
    [SerializeField] private Transform _parent;

    public static ChartDataHolder instance;

    public void Load()
    {
        if (allCharts == default) return;
        
    }

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        if (allCharts == default)
        {
            allCharts = new List<ChartData>();
        }

        string[] chartPaths = Directory.GetFiles(Application.persistentDataPath, "*.level");
        Debug.Log(chartPaths.Length);
        if (chartPaths.Length <= 0) return;
        //if (chartPaths.Length <= allCharts.Count) return;


        foreach (string path in chartPaths)
        {
            ChartData c = JsonUtility.FromJson<ChartData>(File.ReadAllText(path));
            ChartSelection cs = Instantiate(_chartPrefab, _parent);
            foreach (ChartData d in allCharts)
            {
                if (d.name.Equals(c.name))
                {
                    c = d;
                }
            }
            cs.myData = c;
        }
        //allCharts = new();
        
    }
}
