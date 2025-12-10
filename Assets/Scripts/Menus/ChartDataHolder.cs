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
            return;
        }

        
    }
    public void Load()
    {
        if (allCharts == default) return;
        foreach (ChartData c in allCharts)
        {
            charts[c.index].completed = c.completed;
            charts[c.index].score = c.score;
            charts[c.index].percent = c.percent;
            charts[c.index].combo = c.combo;
        }
    }

    public void Save()
    {
        for(int i = 0; i < charts.Count; i++)
        {
            allCharts = new();
            ChartData d = new();
            d.index = i;
            d.score = charts[i].score;
            d.percent = charts[i].percent;
            d.combo = charts[i].combo;
            d.percent = charts[i].percent;
            allCharts.Add(d);
        }
    }
    private void Start()
    {
        /*
        string[] chartPaths = Directory.GetFiles(Application.persistentDataPath, "*.level");
        if (chartPaths.Length <= 0) return;
        if (chartPaths.Length <= allCharts.Count) return;

        foreach (Chart c in allCharts) Destroy(c);

        allCharts = new();

        foreach (string path in chartPaths)
        {
            Chart c = SaveManager.LoadChart(path);
            allCharts.Add(c);
            ChartSelection cs = Instantiate(_chartPrefab, _parent);
            cs.chart = c;
        }
        */
    }
}
