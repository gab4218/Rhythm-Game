using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChartController : MonoBehaviour
{
    public Chart selectedChart;

    public static ChartController instance;

    private float _ts, t;

    private Coroutine _cr;

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
    private void Start()
    {
        EventManager.Subscribe(EventType.Death, EndChart);
        SoundSingleton.instance?.SetMusic(selectedChart.song);
        _cr = StartCoroutine(ChartReader());
        _ts = Time.timeScale;
    }

    private IEnumerator ChartReader()
    {
        foreach (NoteData nData in selectedChart.notes)
        {
            yield return new WaitForSeconds(nData.delayFromLast);
            Note note = Instantiate(nData.note, GameManager.instance.lanes[nData.lane].position, GameManager.instance.lanes[nData.lane].rotation);
            note.speed = nData.noteSpeed;
            //Debug.Log("hate");
        }

    }

    public void EndChart()
    {
        StopCoroutine(_cr);
        Invoke("Reload", 4f);
    }

    public void Reload()
    {
        SceneManager.LoadScene(1);
    }

}
