using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public void SetMusic(Slider slider) => SoundManager.instance.UpdateMusicVolume(slider.value);
    public void SetSFX(Slider slider) => SoundManager.instance.UpdateSFXVolume(slider.value);
    public void SetMaster(Slider slider) => SoundManager.instance.UpdateMasterVolume(slider.value);
}
