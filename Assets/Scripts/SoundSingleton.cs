using System.Collections;
using UnityEngine;

public class SoundSingleton : MonoBehaviour
{
    public static SoundSingleton instance { get; private set; }
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip button;
    public AudioClip hitSound;
    public AudioClip missSound;
    public AudioClip deathSound;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        EventManager.Subscribe(EventType.Hit, Hit);
        EventManager.Subscribe(EventType.Miss, Miss);
        EventManager.Subscribe(EventType.Death, Death);
    }

    public void Button()
    {
        sfxSource.PlayOneShot(button, 1);
    }

    public void Hit()
    {
        sfxSource.PlayOneShot(hitSound, 1);
    }

    public void Miss()
    {
        //sfxSource.PlayOneShot(missSound, 1);
    }

    public void Death()
    {
        //sfxSource.PlayOneShot(deathSound, 1);
        StartCoroutine(DeathMusicFade());
    }

    private IEnumerator DeathMusicFade()
    {
        float t = 0;
        float op = musicSource.pitch;
        float v = musicSource.volume;
        
        while (t < 1)
        {
            musicSource.pitch = Mathf.Lerp(op, 0, t);
            musicSource.volume = Mathf.Lerp(v, 0, t);
            t += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = 0;
    }

    public void SetMusic(AudioClip newSong)
    {
        musicSource.clip = newSong;
        musicSource.Play();
    }
}

