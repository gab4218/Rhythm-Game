using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChartSelection : MonoBehaviour
{
    [SerializeField] private Chart _chart;
    [SerializeField] private TMP_Text _percent, _combo, _score, _name;
    [SerializeField] private GameObject _completed;

    private void Start()
    {
        _completed.SetActive(_chart.completed);
        _percent.text = _chart.percent.ToString("0#.00%");
        _combo.text = _chart.combo.ToString();
        _score.text = _chart.score.ToString();
        _name.text = _chart.name;
    }

    public void SelectChart()
    {
        ChartController.selectedChart = _chart;
        SceneManager.LoadScene("MainGameplay");
    }
}
