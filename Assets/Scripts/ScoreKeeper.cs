using System.Collections;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper instance;

    private int _combo = 0;

    private int _maxCombo = 0;

    private int _score = 0;

    private int _mult = 1;

    public bool highscore = false;

    public bool highcombo = false;

    [SerializeField] private TMPro.TMP_Text _scoreText, _comboText, _multText;
    [SerializeField] private UnityEngine.UI.Image _HighScoreIMG;

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
        EventManager.Subscribe(EventType.Death, Unsub);
        EventManager.Subscribe(EventType.Hit, Score);
        EventManager.Subscribe(EventType.Miss, Miss);
        EventManager.Subscribe(EventType.End, End);
    }

    /// <summary>
    /// Takes int as score added
    /// </summary>
    /// <param name="paramContainer"></param>
    public void Score(params object[] paramContainer)
    {
        int s = (int)paramContainer[0];
        _score += s * _mult;
        _combo++;

        _comboText.text = "Streak: " + _combo.ToString();
        _scoreText.text = "Score: " + _score.ToString();

        if (_score > ChartController.instance.selectedChart.score && !highscore)
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

        while (t < 2f)
        {
            if(t < 0.5f)
            {
                _HighScoreIMG.color = Color.Lerp(invis, Color.white, t/0.5f);
            }
            else if (t < 1)
            {
                _HighScoreIMG.color = Color.Lerp(Color.white, invis, t / 0.5f - 1);
            } 
            else if (t < 1.5f)
            {
                _HighScoreIMG.color = Color.Lerp(invis, Color.white, t / 0.5f - 2);
            }
            else
            {
                _HighScoreIMG.color = Color.Lerp(Color.white, invis, t / 0.5f - 3);
            }
            t += Time.deltaTime;
            yield return null;
        }
    }

    private void Miss(params object[] paramContainer)
    {
        _combo = 0;
        _mult = 1;
        _multText.text = "x" + _mult.ToString();
        _comboText.text = "Streak: " + _combo.ToString();
    }

    private void End(params object[] paramContainer)
    {
        if (_maxCombo > ChartController.instance.selectedChart.combo)
        {
            ChartController.instance.selectedChart.combo = _maxCombo;
        }
        if (highscore)
        {
            ChartController.instance.selectedChart.score = _score;
        }
        ChartController.instance.selectedChart.completed = true;

        Unsub();
    }

    private void Unsub(params object[] paramContainer)
    {
        EventManager.Unsubscribe(EventType.Death, Unsub);
        EventManager.Unsubscribe(EventType.Hit, Score);
        EventManager.Unsubscribe(EventType.Miss, Miss);
        EventManager.Unsubscribe(EventType.End, End);
    }

}
