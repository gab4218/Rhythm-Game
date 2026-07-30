using System.Collections;
using System.Collections.Generic;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class ChartController : MonoBehaviour
{
    public static ChartData selectedChart;

    public static ChartController instance;

    private float _ts = 1;

    private Coroutine _cr;

    [SerializeField] private Transform _chartParent;
    [SerializeField] private Material _flairMat;

    [SerializeField] private Material[] _mountainMaterial;
    [SerializeField] private GameObject[] _bothBGS;
    [SerializeField] private Material _decoMat;
    private float _height;



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
        StartCoroutine(DecoReader());
        SoundSingleton.instance.SetMusic(selectedChart.song);
        _cr = StartCoroutine(ChartReader());
        RemoteConfigService.Instance.FetchCompleted += TSChange;
        _bothBGS[(Menu.selectedVisuals + 1) % 2].SetActive(false);
        
    }

    private IEnumerator SongStarter()
    {
        float t = 0;
        float scaler = 1;
        while (t < 4)
        {
            t += Time.deltaTime * scaler;
            if (PauseScreen.paused) scaler = 0; else scaler = 1;
            yield return null;
        }
        SoundSingleton.instance.PlayMusic();
    }


    private IEnumerator ChartReader()
    {
        float t = 0;
        float scaler = 1f;
        Dictionary<float, Transform> prevPos = new();
        Dictionary<float, Transform> prevPosN = new();
        float prev = 0;

        foreach (NoteData nData in selectedChart.notes)
        {
            t = 0;
            while (t < nData.delayFromLast)
            {
                t += Time.deltaTime * scaler;
                if (PauseScreen.paused) scaler = 0; else scaler = 1;
                yield return null;
            }
            Note note = Instantiate(nData.note).StartPos(GameManager.instance.lanes[nData.lane].position).Speed(-nData.noteSpeed);
            var f = note.transform.forward;
            note.transform.rotation = GameManager.instance.lanes[nData.lane].rotation;
            

            if (nData.flair) StartCoroutine(FlairCR());

            if (prevPos.ContainsKey(nData.noteSpeed))
            {
                if (note is NoteHold && nData.noteSpeed == prev) (note as NoteHold).SetTrail(prevPos[nData.noteSpeed], GameManager.instance.lanes[nData.lane], prevPosN[nData.noteSpeed]);
                else if (note is NoteHold) (note as NoteHold).SetTrail(null, null, null);
                prevPos[nData.noteSpeed] = GameManager.instance.lanes[nData.lane];
                prevPosN[nData.noteSpeed] = note.transform;

            }
            else
            {
                prevPos.Add(nData.noteSpeed, GameManager.instance.lanes[nData.lane]);
                prevPosN.Add(nData.noteSpeed, note.transform);
            }
            prev = nData.noteSpeed;
            note.transform.SetParent(_chartParent);
        }

        yield return new WaitForSecondsRealtime(selectedChart.notes[selectedChart.notes.Length - 1].noteSpeed > 5 ? 9.5f : 5.5f);

        EventManager.TriggerEvent(EventType.End, true);

    }

    private IEnumerator FlairCR()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            _flairMat.SetFloat("_Progress", t);
            yield return null;
        }
        _flairMat.SetFloat("_Progress", 0);
    }

    private IEnumerator DecoReader()
    {
        float t = 0;
        float scaler = 1f;

        foreach (DecoData dData in selectedChart.deco)
        {
            t = 0;
            while (t < dData.delayFromLast)
            {
                t += Time.deltaTime * scaler;
                _mountainMaterial[Menu.selectedVisuals].SetFloat("_Height", Mathf.Lerp(_mountainMaterial[Menu.selectedVisuals].GetFloat("_Height"), _height, 1f - Mathf.Pow(0.2f, Time.deltaTime)));
                if (PauseScreen.paused) scaler = 0; else scaler = 1;
                yield return null;
            }
            if (dData.obj != null)
            { 
                var d = Instantiate(dData.obj, transform);
                int r = Random.Range(0, 2);
                float rand;
                if (r == 0) rand = Random.Range(10f, 25f);
                else rand = Random.Range(-10f, -25f);
                d.transform.position = new Vector3(rand, -3.66f, GameManager.instance.lanes[0].position.z);
                d.speed = dData.speed;
            }
            else
            {
                _height = dData.mountainHeight;
            }
        }
        yield return new WaitForSecondsRealtime(selectedChart.deco[selectedChart.deco.Length - 1].speed > 5 ? 2f : (selectedChart.deco[selectedChart.deco.Length - 1].speed < 5 ? 8f : 4f));
        while (_mountainMaterial[Menu.selectedVisuals].GetFloat("_Height") > 0.1)
        {
            _mountainMaterial[Menu.selectedVisuals].SetFloat("_Height", Mathf.Lerp(_mountainMaterial[Menu.selectedVisuals].GetFloat("_Height"), 0, 1f - Mathf.Pow(0.5f, Time.deltaTime)));
            yield return null;
        }
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
