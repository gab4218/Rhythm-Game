using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName ="Chart")]
public class Chart : ScriptableObject
{
    public NoteData[] notes;
    public AudioClip song;
    public float bpm;
    public int score = 0;
    public int combo = 0;
    public float percent = 0;
    public bool completed = false;
    public void ResetChartData()
    {
        completed = false;
        score = 0;
        combo = 0;
        percent = 0;
    }
}

public struct ChartData
{
    public NoteData[] notes;
    public string name;
    public AudioClip song;
    public int score;
    public int combo;
    public float percent;
    public bool completed;
    public int index;
}
