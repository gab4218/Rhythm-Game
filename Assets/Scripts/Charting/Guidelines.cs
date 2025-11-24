using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Guidelines")]
public class Guidelines : ScriptableObject
{

    public List<float> delays;
    public AudioClip song;

    public void AddGuide(float timeFromLast)
    {
        delays.Add(timeFromLast);
    }

    public void ClearGuides()
    {
        delays = new List<float>(); 
    }

}
