using UnityEngine;

public class ResetChartData : MonoBehaviour
{
    [SerializeField] private Chart[] _allCharts;

    public void ResetAll()
    {
        foreach (Chart chart in _allCharts)
        {
            chart.ResetChartData();
        }
    }
}
