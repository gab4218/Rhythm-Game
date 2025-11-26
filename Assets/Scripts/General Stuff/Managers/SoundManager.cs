using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public UnityEngine.Audio.AudioMixer mixer;
    public UnityEngine.UI.Slider masterSlider, sfxSlider, musicSlider;

    private void Awake()
    {
        float t;
        mixer.GetFloat("MasterVolume", out t);
        masterSlider.value = t;
        mixer.GetFloat("MusicVolume", out t);
        musicSlider.value = t;
        mixer.GetFloat("SFXVolume", out t);
        sfxSlider.value = t;
    }

    public void UpdateMasterVolume()
    {
        mixer.SetFloat("MasterVolume", masterSlider.value);
        if (!SoundSingleton.instance.sfxSource.isPlaying)
        {
            SoundSingleton.instance.Button();
        }
    }

    public void UpdateMusicVolume()
    {
        mixer.SetFloat("MusicVolume", musicSlider.value);
    }

    public void UpdateSFXVolume()
    {
        mixer.SetFloat("SFXVolume", sfxSlider.value);
        if (!SoundSingleton.instance.sfxSource.isPlaying)
        {
            SoundSingleton.instance.Button();
        }
    }
}
