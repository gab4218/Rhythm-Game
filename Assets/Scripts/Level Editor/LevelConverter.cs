using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class LevelConverter
{
    public static List<EditorNote> SortByTime(this List<EditorNote> level)
    {
        List<EditorNote> sortedLevel = new();

        EditorNote buffer = level[0];
        bool found = false;
        for (int i = 0; i < level.Count; i++)
        {
            foreach (EditorNote note in sortedLevel)
            {
                if (level[i].transform.position.x > note.transform.position.x) continue;
                found = true;
                sortedLevel.Insert(sortedLevel.IndexOf(note), level[i]);
            }
            if (found) found = false;
            else sortedLevel.Add(level[i]);
        }

        return sortedLevel;
    }

    
    public static List<EditorNote> SortByTime(this List<EditorNote> level, Dictionary<EditorNote, float> times)
    {
        List<EditorNote> ordered = level;

        ordered.Sort((a, b) => times[a].CompareTo(times[b])); //lambda expression sacada de https://discussions.unity.com/t/sorting-a-dictionary-key-value/809858/4

        return ordered;
    }


    public static Dictionary<EditorNote, float> CalculateTimes(this List<EditorNote> level, float scaler = 1)
    {
        Dictionary<EditorNote, float> times = new();

        times.Add(level[0], 0);

        for(int i = 1; i < level.Count; i++)
        {
            float speedModifier = 0;
            switch (level[i].data.speed)
            {
                case EditorNoteSpeeds.Half:
                    speedModifier = -4f;
                    break;
                case EditorNoteSpeeds.Regular:
                    speedModifier = 0f;
                    break;
                case EditorNoteSpeeds.Double:
                    speedModifier = 2f;
                    break;
                default:
                    speedModifier = 0;
                    break;
            }

            var left = Mathf.RoundToInt(level[i-1].transform.position.x);
            var right = Mathf.RoundToInt(level[i].transform.position.x);

            times.Add(level[i], (right - left) * scaler + speedModifier);
        }

        
        return times;
    }

    public static Dictionary<EditorNote, float> CalculateSpeeds(this List<EditorNote> level)
    {
        Dictionary <EditorNote, float> speeds = new();

        foreach (var note in level)
        {
            speeds.Add(note, 0);
            switch (note.data.speed)
            {
                case EditorNoteSpeeds.Half:
                    speeds[note] = 2.5f;
                    break;
                case EditorNoteSpeeds.Regular:
                    speeds[note] = 5f;
                    break;
                case EditorNoteSpeeds.Double:
                    speeds[note] = 10f;
                    break;
                default:
                    speeds[note] = 5f;
                    break;
            }
        }

        return speeds;

    }

    public static Dictionary<EditorNote, int> CalculateLanes(this List<EditorNote> level, float scaler)
    {
        Dictionary<EditorNote, int> lanes = new();

        foreach (var note in level)
        {
            lanes.Add(note, 0);
            int rounded = (int)(note.transform.localPosition.y / scaler);
            if (rounded == 0) continue;
            //if (rounded == )
        }


        return lanes;
    }


}
