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
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        _startColor1 = Camera.main.backgroundColor;
        //_startColor1 = material.GetColor("_downColor2");
        //_startColor2 = material.GetColor("_sideColor2");
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
        sfxSource?.PlayOneShot(button, 1);
    }

    public void Hit(params object[] paramContainer)
    {
        if (sfxSource == null) return;
        sfxSource.pitch = Random.Range(0.8f, 1.2f);
        sfxSource.PlayOneShot(hitSound, 1);
        Camera.main.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        //material.SetColor("_sideColor2", Color.white);
        //material.SetColor("_downColor2", Color.white);
    }

    public void Miss(params object[] paramContainer)
    {
        Debug.Log(sfxSource.transform.name);
        sfxSource.pitch = Random.Range(0.8f, 1.2f);
        sfxSource.PlayOneShot(missSound, 1);
        Camera.main.backgroundColor = Color.red;
        //material.SetColor("_sideColor2", Color.red);
        //material.SetColor("_downColor2", Color.red);
    }

    private void FixedUpdate()
    {
        Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, _startColor1, 0.2f);
        //material.SetColor("_downColor2", Color.Lerp(material.GetColor("_downColor2"), _startColor1, 0.2f));
        //material.SetColor("_sideColor2", Color.Lerp(material.GetColor("_sideColor2"), _startColor2, 0.2f));
    }

    private void OnDestroy()
    {
        End();
        instance = null;
    }

    public void Death(params object[] paramContainer)
    {
        //sfxSource.PlayOneShot(deathSound, 1);
        //material.SetColor("_downColor2", _startColor1);
        //material.SetColor("_sideColor2", _startColor2);
        Camera.main.backgroundColor = _startColor1;
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
    }

    public void PlayMusic() => musicSource.Play();


    public void StopMusic()
    {
        musicSource.Stop();
    }
}

