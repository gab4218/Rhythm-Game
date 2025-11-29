using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ChartSelection : MonoBehaviour
{
    public Chart chart;
    [SerializeField] private TMP_Text _percent, _combo, _score, _name;
    [SerializeField] private GameObject _completed;
    //[SerializeField] private Button _button;
    [SerializeField] private AudioClip clip;
    public ChartData myData = new();



    private void Start()
    {
        if(ChartDataHolder.allCharts == default)
        {
            ChartDataHolder.allCharts = new();
        }
        
        if(myData.percent > 0)
        {
            _completed.SetActive(myData.completed);
            _percent.text = myData.percent.ToString("0#.00%");
            _combo.text = myData.combo.ToString();
            _score.text = myData.score.ToString();
            _name.text = myData.name;
        }
        bool found = false;
        if (chart != default && myData.percent <= 0)
        {
            myData = new ChartData
            {
                notes = chart.notes,
                name = chart.name,
                song = chart.song,
                completed = chart.completed,
                combo = chart.combo,
                score = chart.score,
                percent = chart.percent
            };
            
            foreach (ChartData d in ChartDataHolder.allCharts)
            {
                if (d.name.Equals(myData.name))
                {
                    myData = d;
                    found = true;
                    break;
                }
            }
            if(!found) ChartDataHolder.allCharts.Add(myData);
        }

        if (!found)
        {
            foreach (ChartData d in ChartDataHolder.allCharts)
            {
                if (d.name.Equals(myData.name))
                {
                    myData = d;
                }
            }
        }

        if (myData.name == ChartController.selectedChart.name && ChartController.selectedChart.percent > 0)
        {
            myData = ChartController.selectedChart;
            Debug.Log(ChartController.selectedChart.percent + "asdasda");
            for (int i = 0; i < ChartDataHolder.allCharts.Count; i++)
            {
                if (myData.name == ChartDataHolder.allCharts[i].name) ChartDataHolder.allCharts[i] = myData;
            }
        }

        _completed.SetActive(myData.completed);
        _percent.text = myData.percent.ToString("0#.00%");
        _combo.text = myData.combo.ToString();
        _score.text = myData.score.ToString();
        _name.text = myData.name;
    }

    public void SelectChart()
    {
        if (StaminaManager.instance.UseStamina())
        {
            ChartController.selectedChart = myData;
            SceneManager.LoadScene("MainGameplay");
        }
        else
        {
            SoundSingleton.instance.sfxSource.PlayOneShot(clip);
        }
    }
}
