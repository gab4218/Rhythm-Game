using System.Collections;
using UnityEditor.Hardware;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChartController : MonoBehaviour
{
    public Chart selectedChart;

    public static ChartController instance;

    //private float _ts, t;

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
        EventManager.Subscribe(EventType.End, EndChart);

        SoundSingleton.instance?.SetMusic(selectedChart.song);
        _cr = StartCoroutine(ChartReader());
    }


    

    private IEnumerator ChartReader()
    {
        foreach (NoteData nData in selectedChart.notes)
        {
            yield return new WaitForSeconds(nData.delayFromLast);
            Note note = Instantiate(nData.note).StartPos(GameManager.instance.lanes[nData.lane].position, GameManager.instance.lanes[nData.lane].rotation).Speed(nData.noteSpeed);
        }
        EventManager.TriggerEvent(EventType.End);
    }

    public void EndChart(params object[] paramContainer)
    {
        StopCoroutine(_cr);
        Invoke("Reload", 4f);
        EventManager.Unsubscribe(EventType.Death, EndChart);
        EventManager.Unsubscribe(EventType.End, EndChart);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
