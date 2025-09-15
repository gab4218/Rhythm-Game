using System.Collections;
using UnityEngine;

public class ChartController : MonoBehaviour
{
    public Chart selectedChart;

    public static ChartController instance;



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
        StartCoroutine(ChartReader());
    }

    private IEnumerator ChartReader()
    {
        foreach (NoteData nData in selectedChart.notes)
        {
            yield return new WaitForSeconds(nData.delayFromLast);
            Note note = Instantiate(nData.note, GameManager.instance.lanes[nData.lane].position, GameManager.instance.lanes[nData.lane].rotation);
            note.speed = nData.noteSpeed;
            Debug.Log("hate");
        }

    }

    public void EndChart()
    {
        Debug.Log("dead");
    }

}
