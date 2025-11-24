using System.Collections;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class ChartController : MonoBehaviour
{
    public static Chart selectedChart;

    public static ChartController instance;

    private float _ts = 1;

    private Coroutine _cr;

    [SerializeField] private Transform _chartParent;




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
        //Debug.Log(selectedChart.name);
    }

    private void Start()
    {
        EventManager.Subscribe(EventType.Death, Death);
        EventManager.Subscribe(EventType.End, EndChart);

        SoundSingleton.instance?.SetMusic(selectedChart.song);
        _cr = StartCoroutine(ChartReader());
        RemoteConfigService.Instance.FetchCompleted += TSChange;
    }


    

    private IEnumerator ChartReader()
    {
        foreach (NoteData nData in selectedChart.notes)
        {
            yield return new WaitForSeconds(nData.delayFromLast);
            Note note = Instantiate(nData.note).StartPos(GameManager.instance.lanes[nData.lane].position).Speed(-nData.noteSpeed);
            note.transform.SetParent(_chartParent);
        }

        while (SoundSingleton.instance.musicSource.time < selectedChart.song.length)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1.5f);

        EventManager.TriggerEvent(EventType.End);

    }

    public void Death(params object[] paramContainer)
    {
        StopCoroutine(_cr);
        StartCoroutine(SlowDowner());
        RemoteConfigService.Instance.FetchCompleted -= TSChange;
        EventManager.Unsubscribe(EventType.Death, Death);
        EventManager.Unsubscribe(EventType.End, EndChart);
    }

    private IEnumerator SlowDowner()
    {
        float t = 0;
        while (t < 1f)
        {
            Time.timeScale = Mathf.Lerp(1, 0, t);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 0;
    }

    public void EndChart(params object[] paramContainer)
    {
        StopCoroutine(_cr);
        RemoteConfigService.Instance.FetchCompleted -= TSChange;
        EventManager.Unsubscribe(EventType.Death, Death);
        EventManager.Unsubscribe(EventType.End, EndChart);
    }

    private void TSChange(ConfigResponse configResponse)
    {
        _ts = RemoteConfigService.Instance.appConfig.GetFloat("PlaybackSpeed");
        if (_ts != Time.timeScale)
        {
            Time.timeScale = _ts;
            SoundSingleton.instance.musicSource.pitch = _ts;
        }
    }

}
