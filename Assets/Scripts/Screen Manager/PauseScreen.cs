using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour, IScreen
{
    private Button[] _buttons;

    [SerializeField] private string _optionsName = "Options";
    [SerializeField] private string _menuName = "Menu";

    public static bool paused = false;

    private void Awake()
    {
        _buttons = GetComponentsInChildren<Button>();

        foreach (Button button in _buttons)
        {
            button.interactable = false;
        }
        paused = true;
    }

    public void Activate()
    {
        foreach (Button button in _buttons)
        {
            button.interactable = true;
        }
    }

    public void Deactivate()
    {
        foreach (Button button in _buttons)
        {
            button.interactable = false;
        }
    }

    public void Free()
    {
        paused = false;
        Destroy(gameObject);
    }

    public void Options() => ScreenManager.instance.Push(_optionsName);

    public void Menu()
    {
        EventManager.TriggerEvent(EventType.End, false);
        paused = false;
        SceneManager.LoadScene(_menuName);
    }

    public void Resume() => ScreenManager.instance.Pop();

    
    
    //public void ChangeLanguage() =>

}


