using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private string type;
    void Start()
    {
        slider.SetValueWithoutNotify(SoundManager.instance.GetVolume(type));
    }
}
