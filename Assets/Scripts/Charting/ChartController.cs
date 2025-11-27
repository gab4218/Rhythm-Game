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

        StartCoroutine(SongStarter());
        _cr = StartCoroutine(ChartReader());
        RemoteConfigService.Instance.FetchCompleted += TSChange;
    }

    private IEnumerator SongStarter()
    {
        yield return new WaitForSeconds(4);
        SoundSingleton.instance?.SetMusic(selectedChart.song);
    }
    

    private IEnumerator ChartReader()
    {
        float t = 0;
        float scaler = 1f;
        foreach (NoteData nData in selectedChart.notes)
        {
            t = 0;
            while(t < nData.delayFromLast)
            {
                t += Time.deltaTime * scaler;
                if (PauseScreen.paused) scaler = 0; else scaler = 1;
                yield return null;
            }
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
