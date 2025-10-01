using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Chart[] allCharts;
    public Transform[] lanes;
    public PlayerModel player;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    

    public void ResetAll()
    {
        foreach (Chart chart in allCharts)
        {
            chart.ResetChartData();
        }
    }
}
