using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SoundManager : MonoBehaviour
{
    public UnityEngine.Audio.AudioMixer mixer;
    //public UnityEngine.UI.Slider masterSlider, sfxSlider, musicSlider;
    public static SoundManager instance;
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

    public void UpdateMasterVolume(float value)
    {
        mixer.SetFloat("MasterVolume", value);
        if (!SoundSingleton.instance.sfxSource.isPlaying)
        {
            SoundSingleton.instance.Button();
        }
    }

    public void UpdateMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume", value);
        if (value <= -25f) mixer.SetFloat("MusicVolume", -80f); 
    }

    public void UpdateSFXVolume(float value)
    {
        mixer.SetFloat("SFXVolume", value);
        if(value <= -25f) mixer.SetFloat("SFXVolume", -80f);
        if (!SoundSingleton.instance.sfxSource.isPlaying)
        {
            SoundSingleton.instance.Button();
        }
    }

    public float GetVolume(string type)
    {
        mixer.GetFloat(type, out float t);
        return t;
    }

    public float GetMusicVolume()
    {
        mixer.GetFloat("MusicVolume", out float t);
        return t;
    }
    public float GetSFXVolume()
    {
        mixer.GetFloat("SFXVolume", out float t);
        return t;
    }
    public float GetMasterVolume()
    {
        mixer.GetFloat("MusicVolume", out float t);
        return t;
    }
}
