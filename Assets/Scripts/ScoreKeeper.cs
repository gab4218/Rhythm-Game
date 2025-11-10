using System.Collections;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;

    private int _combo = 0;
    private int _maxCombo = 0;
    private int _score = 0;
    private int _mult = 1;
    private int _hits = 0;
    private int _totalNotes = 0;
    private float _percent = 0;

    public bool highscore = false;
    public bool highcombo = false;

    [SerializeField] private TMPro.TMP_Text _scoreText, _comboText, _multText, _percentText, _endScoreText, _endComboText, _endPercentText;
    [SerializeField] private UnityEngine.UI.Image _HighScoreIMG, _HighScoreEndIMG;
    [SerializeField] private GameObject _win, _lose;
    


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

        EventManager.Subscribe(EventType.Death, End);
        EventManager.Subscribe(EventType.Hit, Score);
        EventManager.Subscribe(EventType.Miss, Miss);
        EventManager.Subscribe(EventType.End, Win);
    }

    /// <summary>
    /// Function takes int as score added
    /// </summary>
    /// <param name="paramContainer"></param>
    public void Score(params object[] paramContainer)
    {
        int s = (int)paramContainer[0];
        _score += s * _mult;
        _combo++;
        _hits++;
        _totalNotes++;
        _comboText.text = "Streak: " + _combo.ToString();
        _scoreText.text = "Score: " + _score.ToString();

        _percent = ((float)_hits) / _totalNotes;
        _percentText.text = _percent.ToString("0#.00%");

        if (_score > ChartController.selectedChart.score && !highscore)
        {
            highscore = true;
            StartCoroutine(HighScore());
        }

        if (_combo > 30)
        {
            _mult = 4;
        }
        else if (_combo > 20)
        {
            _mult = 3;
        }
        else if (_combo > 10)
        {
            _mult = 2;
        }

        _multText.text = "x" + _mult.ToString();

    }

    private IEnumerator HighScore()
    {
        float t = 0;
        Color invis = new Color(1, 1, 1, 0);

        while (t < 0.5f)
        {
            _HighScoreIMG.color = Color.Lerp(invis, Color.white, t / 0.5f);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        while (t < 0.5f)
        {
            _HighScoreIMG.color = Color.Lerp(Color.white, invis, t / 0.5f);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        while (t < 0.5f)
        {
            _HighScoreIMG.color = Color.Lerp(invis, Color.white, t / 0.5f);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0;
        while (t < 0.5f)
        {
            _HighScoreIMG.color = Color.Lerp(Color.white, invis, t / 0.5f);
            t += Time.deltaTime;
            yield return null;
        }
        _HighScoreIMG.gameObject.SetActive(false);
    }

    private void Miss(params object[] paramContainer)
    {
        _combo = 0;
        _mult = 1;
        _totalNotes++;
        _multText.text = "x" + _mult.ToString();
        _comboText.text = "Streak: " + _combo.ToString();
    }

    private void Win(params object[] paramContainer)
    {
        ChartController.selectedChart.completed = true;
        _win.SetActive(true);
        _lose.SetActive(false);
        End();
    }

    private void End(params object[] paramContainer)
    {
        if (_maxCombo > ChartController.selectedChart.combo)
        {
            ChartController.selectedChart.combo = _maxCombo;
        }
        if (highscore)
        {
            ChartController.selectedChart.score = _score;
            ChartController.selectedChart.percent = _percent;
            _HighScoreEndIMG.gameObject.SetActive(true);
        }

        _endComboText.text = _maxCombo.ToString();
        _endPercentText.text = _percent.ToString("0#.00%");
        _endScoreText.text = _score.ToString();

        MoneyManager.money += _score / 4;

        EventManager.Unsubscribe(EventType.Death, End);
        EventManager.Unsubscribe(EventType.Hit, Score);
        EventManager.Unsubscribe(EventType.Miss, Miss);
        EventManager.Unsubscribe(EventType.End, Win);
    }

}
