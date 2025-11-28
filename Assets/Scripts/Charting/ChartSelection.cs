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

    
    private void Start()
    {
        _completed.SetActive(chart.completed);
        _percent.text = chart.percent.ToString("0#.00%");
        _combo.text = chart.combo.ToString();
        _score.text = chart.score.ToString();
        _name.text = chart.name;
    }

    public void SelectChart()
    {
        if (StaminaManager.instance.UseStamina())
        {
            ChartController.selectedChart = chart;
            SceneManager.LoadScene("MainGameplay");
        }
        else
        {
            SoundSingleton.instance.sfxSource.PlayOneShot(clip);
        }
    }
}
