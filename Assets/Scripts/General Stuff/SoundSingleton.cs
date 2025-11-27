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
    public Material material;
    private Color _startColor1, _startColor2;

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
        _startColor1 = material.GetColor("_downColor2");
        _startColor2 = material.GetColor("_sideColor2");
    }

    private void Start()
    {
        EventManager.Subscribe(EventType.Hit, Hit);
        EventManager.Subscribe(EventType.Miss, Miss);
        EventManager.Subscribe(EventType.Death, Death);
        EventManager.Subscribe(EventType.End, End);
    }

    public void Button()
    {
        sfxSource.PlayOneShot(button, 1);
    }

    public void Hit(params object[] paramContainer)
    {
        sfxSource.pitch = Random.Range(0.8f, 1.2f);
        sfxSource.PlayOneShot(hitSound, 1);
        material.SetColor("_sideColor2", Color.white);
        material.SetColor("_downColor2", Color.white);
    }

    public void Miss(params object[] paramContainer)
    {
        sfxSource.pitch = Random.Range(0.8f, 1.2f);
        sfxSource.PlayOneShot(missSound, 1);
        material.SetColor("_sideColor2", Color.red);
        material.SetColor("_downColor2", Color.red);
    }

    private void FixedUpdate()
    {
        material.SetColor("_downColor2", Color.Lerp(material.GetColor("_downColor2"), _startColor1, 0.2f));
        material.SetColor("_sideColor2", Color.Lerp(material.GetColor("_sideColor2"), _startColor2, 0.2f));
    }

    public void Death(params object[] paramContainer)
    {
        //sfxSource.PlayOneShot(deathSound, 1);
        material.SetColor("_downColor2", _startColor1);
        material.SetColor("_sideColor2", _startColor2);
        StartCoroutine(DeathMusicFade());
        End();
    }

    private void End(params object[] paramContainer)
    {
        EventManager.Unsubscribe(EventType.Hit, Hit);
        EventManager.Unsubscribe(EventType.Miss, Miss);
        EventManager.Unsubscribe(EventType.Death, Death);
        EventManager.Unsubscribe(EventType.End, End);
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
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        musicSource.volume = 0;
    }

    public void SetMusic(AudioClip newSong)
    {
        musicSource.clip = newSong;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}

