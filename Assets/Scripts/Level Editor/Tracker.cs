using UnityEngine;

public class Tracker : MonoBehaviour
{
    public AudioClip tick;
    private void OnTriggerEnter2D(Collider2D other)
    {
        SoundSingleton.instance.sfxSource.PlayOneShot(tick);
    }
}
