using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName ="Chart")]
public class Chart : ScriptableObject
{
    public NoteData[] notes;
    public AudioClip song;
    public float bpm;
    public int score = 0;
    public bool completed = false;

    public void ResetChartData()
    {
        completed = false;
        score = 0;
    }
}
