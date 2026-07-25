using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName ="Chart")]
public class Chart : ScriptableObject
{
    public NoteData[] notes;
    public DecoData[] deco;
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
    public DecoData[] deco;
    public string name;
    public AudioClip song;
    public int score;
    public int combo;
    public float percent;
    public bool completed;
    public int index;

}
[System.Serializable]
public struct DecoData
{
    public ChartDeco obj;
    public float delayFromLast;
    public float speed;
    public float mountainHeight;
}